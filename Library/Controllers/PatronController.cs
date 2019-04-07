using System.Collections.Generic;
using System.Linq;
using Library.Models.Patron;
using LibraryData;
using LibraryData.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class PatronController : Controller
    {
        private readonly IPatron _patron;

        public PatronController(IPatron patron)
        {
            _patron = patron;
        }

        public IActionResult Index()
        {
            var allPatrons = _patron.GetAll();
            var patronModels = allPatrons.Select(p => new PatronDetailModel
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                CheckoutHistory = _patron.GetCheckoutHistory(p.Id),
                HomeLibraryBranch = p.HomeLibraryBranch.Name,
                AssetsCheckedOut = _patron.GetCheckouts(p.Id),
                OverdueFees = p.LibraryCard.Fees,
                Holds = _patron.GetHolds(p.Id),
                LibraryCardId = p.LibraryCard.Id,
                MemberSince = p.LibraryCard.Created,
                Telephone = p.TelephoneNumber
            }).ToList();

            var model = new PatronIndexModel
            {
                Patrons = patronModels
            };
            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var p = _patron.Get(id);
            var model = new PatronDetailModel
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                CheckoutHistory = _patron.GetCheckoutHistory(p.Id),
                HomeLibraryBranch = p.HomeLibraryBranch.Name,
                AssetsCheckedOut = _patron.GetCheckouts(p.Id).ToList() ?? new List<Checkout>(),
                OverdueFees = p.LibraryCard.Fees,
                Holds = _patron.GetHolds(p.Id),
                LibraryCardId = p.LibraryCard.Id,
                MemberSince = p.LibraryCard.Created,
                Telephone = p.TelephoneNumber
            };

            return View(model);
        }
    }
}
