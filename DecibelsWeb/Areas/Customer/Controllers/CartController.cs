﻿using Decibels.DataAccess.Repository.IRepository;
using Decibels.Models;
using Decibels.Models.ViewModels;
using Decibels.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DecibelsWeb.Areas.Customer.Controllers
{
	// populates and displays shopping cart UI
	[Area("customer")]
	[Authorize]
	public class CartController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		[BindProperty]  // submitted data in Summary View will populate this ViewModel
		public ShoppingCartVM ShoppingCartVM { get; set; }

		public CartController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			// get userID of the logged in user
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			ShoppingCartVM = new()
			{
				ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(
					u => u.ApplicationUserId == userId, includeProperties: "Product"),
				OrderHeader = new()  // so OrderHeader != null and does not throw exception
			};

			foreach (var cart in ShoppingCartVM.ShoppingCartList)
			{
				cart.Price = GetProductPrice(cart);
				ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Quantity);
			}

			return View(ShoppingCartVM);
		}

		public IActionResult Summary()
		{
			// get userID of the logged in user
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			ShoppingCartVM = new()
			{
				ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(
					u => u.ApplicationUserId == userId, includeProperties: "Product"),
				OrderHeader = new()  // so OrderHeader != null and does not throw exception
			};

			ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

			ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
			ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
			ShoppingCartVM.OrderHeader.Street = ShoppingCartVM.OrderHeader.ApplicationUser.Street;
			ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
			ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
			ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;


			foreach (var cart in ShoppingCartVM.ShoppingCartList)
			{
				cart.Price = GetProductPrice(cart);
				ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Quantity);
			}
			return View(ShoppingCartVM);
		}

		[HttpPost]
		[ActionName("Summary")]
		public IActionResult SummaryPOST()
		{
			// get userID of the logged in user
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			// already populated in the database thanks to [BindProperty], so no need to create a new instance like in GET
			// nor explicitly map the properties (ex: ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name)
			ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(
					u => u.ApplicationUserId == userId, includeProperties: "Product");

			ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
			ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

			ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

			foreach (var cart in ShoppingCartVM.ShoppingCartList)
			{
				cart.Price = GetProductPrice(cart);
				ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Quantity);
			}

			if (ShoppingCartVM.OrderHeader.ApplicationUser.CompanyId.GetValueOrDefault() == 0)
			{
				// regular customer account so need to get payment
				ShoppingCartVM.OrderHeader.PaymentStatus = StaticDetails.PaymentStatusPending;
				ShoppingCartVM.OrderHeader.OrderStatus = StaticDetails.StatusPending;
			}
			else
			{
				// company user
				ShoppingCartVM.OrderHeader.PaymentStatus = StaticDetails.PaymentStatusDelayedPayment;
				ShoppingCartVM.OrderHeader.OrderStatus = StaticDetails.StatusPending;
			}

			_unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
			_unitOfWork.Save();
			foreach (var cart in ShoppingCartVM.ShoppingCartList)
			{
				OrderDetail orderDetail = new()
				{
					ProductId = cart.ProductId,
					OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
					Price = cart.Price,
					Quantity = cart.Quantity,
				};
				_unitOfWork.OrderDetail.Add(orderDetail);
				_unitOfWork.Save();
			}


			return View(ShoppingCartVM);
		}

		public IActionResult Plus(int cartId)
		{
			var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
			cartFromDb.Quantity += 1;
			_unitOfWork.ShoppingCart.Update(cartFromDb);
			_unitOfWork.Save();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Minus(int cartId)
		{
			var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
			if (cartFromDb.Quantity <= 1)
			{
				// remove product from cart
				_unitOfWork.ShoppingCart.Remove(cartFromDb);
			}
			else
			{
				cartFromDb.Quantity -= 1;
				_unitOfWork.ShoppingCart.Update(cartFromDb);
			}
			_unitOfWork.Save();
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Remove(int cartId)
		{
			var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
			_unitOfWork.ShoppingCart.Remove(cartFromDb);
			_unitOfWork.Save();
			return RedirectToAction(nameof(Index));
		}


		private decimal GetProductPrice(ShoppingCart shoppingCart)
		{
			return shoppingCart.Product.Price;
		}
	}
}
