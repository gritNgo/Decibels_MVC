using Decibels.DataAccess.Repository;
using Decibels.DataAccess.Repository.IRepository;
using Decibels.Models;
using Decibels.Models.ViewModels;
using Decibels.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DecibelsWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int orderId)
        {
            // based on orderId populate VM
            OrderVM = new()
            {
                OrderHeader = _unitOfWork.OrderHeader
                .Get(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetAll(u=>u.OrderHeaderId==orderId, includeProperties:"Product")
            };
            return View(OrderVM);
        }

        [HttpPost]
        [Authorize(Roles = StaticDetails.Role_Admin+","+StaticDetails.Role_Employee)]
        public IActionResult UpdateOrderDetails()
        {
            var orderHeaderFromDb = _unitOfWork.OrderHeader.Get(u=>u.Id==OrderVM.OrderHeader.Id); // the id passed in the hidden input field of the view
            // map the fields to update
            orderHeaderFromDb.Name = OrderVM.OrderHeader.Name;
            orderHeaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderHeaderFromDb.Street = OrderVM.OrderHeader.Street;
            orderHeaderFromDb.City = OrderVM.OrderHeader.City;
            orderHeaderFromDb.State = OrderVM.OrderHeader.State;
            orderHeaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;

            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.Courier))
            {
                orderHeaderFromDb.Courier = OrderVM.OrderHeader.Courier;
            }
            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.TrackingNumber))
            {
                orderHeaderFromDb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            }
            _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
            _unitOfWork.Save();

            TempData["Success"] = "Order Details Updated Successfully.";

            return RedirectToAction(nameof(Details), new { orderId = orderHeaderFromDb.Id}); // redirect and update Id in database
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            // change from List to IEnumerable for the filtering to work
            IEnumerable<OrderHeader> objOrderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();

            switch (status)
            {
                case "pending":
                    objOrderHeaders = objOrderHeaders.Where(u => u.PaymentStatus == StaticDetails.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == StaticDetails.StatusInProcess);
                    break;
                case "completed":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == StaticDetails.StatusShipped);
                    break;
                case "approved":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == StaticDetails.StatusApproved);
                    break;
                default:
                    break;
            }
            return Json(new { data = objOrderHeaders });
        }

        #endregion
    }
}


