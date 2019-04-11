using System.Linq;
using Library.Models.Catalog;
using Library.Models.Checkout;
using LibraryData;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class CatalogController:Controller
    {
        private readonly ILibraryAsset _assets;
        private readonly ICheckout _checkouts;

        public CatalogController(ILibraryAsset assets,ICheckout checkout)
        {
            _assets = assets;
            _checkouts = checkout;
        }

        public IActionResult Index()
        {
            var assetModels = _assets.GetAll();

            var listingResult = assetModels
                .Select(result => new AssetIndexListingModel
                {
                    Id = result.Id,
                    ImageUrl = result.ImageUrl,
                    AuthorOrDirector = _assets.GetAuthorOrDirector(result.Id),
                    DeweyCallNumber = _assets.GetDeweyIndex(result.Id),
                    Title = result.Title,
                    Type = _assets.GetType(result.Id)
                });
            var model = new AssetIndexModel
            {
                Assets = listingResult
            };

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var asset = _assets.GetById(id);
            var currentHolds = _checkouts.GetCurrentHolds(id)
                .Select(a => new AssetHoldModel
                {
                    HoldPlaced=_checkouts.GetCurrentHoldPlaced(a.Id).ToString("d"),
                    PatronName = _checkouts.GetCurrentHoldPatronName(a.Id)
                });
            var model = new AssetDetailModel
            {
                AssetId = id,
                PatronName = _checkouts.GetCurrentCheckoutPatron(id),
                Title = asset.Title,
                Type=_assets.GetType(id),
                Year = asset.Year,
                Cost = asset.Cost,
                Status = asset.Status.Name,
                ImageUrl = asset.ImageUrl,
                AuthorOrDirector = _assets.GetAuthorOrDirector(asset.Id),
                CurrentLocation = _assets.GetCurrentLocation(asset.Id).Name,
                DeweyCallNumber = _assets.GetDeweyIndex(asset.Id),
                ISBN = _assets.GetIsbn(asset.Id),
                CheckoutHistory = _checkouts.GetCheckoutHistory(id),
                LatestCheckout = _checkouts.GetLatestCheckout(id),
                CurrentHolds = currentHolds
            };

            return View(model);
        }

        public IActionResult Checkout(int id)
        {
            var asset = _assets.GetById(id);

            var model = new CheckoutModel
            {
                AssetId = id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                IsCheckedOut = _checkouts.IsCheckedOut(id)
            };

            return View(model);
        }

        public IActionResult CheckIn(int id)
        {
            _checkouts.CheckInItem(id);
            return RedirectToAction("Detail", new { id = id });
        }

        public IActionResult Hold(int id)
        {
            var asset = _assets.GetById(id);

            var model = new CheckoutModel
            {
                AssetId = id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                IsCheckedOut = _checkouts.IsCheckedOut(id),
                HoldCount = _checkouts.GetCurrentHolds(id).Count()
            };

            return View(model);
        }


        public IActionResult MarkLost(int id)
        {
            _checkouts.MarkLost(id);
            return RedirectToAction("Detail", new { id = id });
        }

        public IActionResult MarkFound(int id)
        {
            _checkouts.MarkFound(id);
            return RedirectToAction("Detail", new { id = id });
        }

        [HttpPost]
        public IActionResult PlaceCheckout(int id, int libraryCardId)
        {
            _checkouts.CheckOutItem(id, libraryCardId);
            return RedirectToAction("Detail", new { id = id });
        }

        [HttpPost]
        public IActionResult PlaceHold(int id, int libraryCardId)
        {
            _checkouts.PlaceHold(id, libraryCardId);
            return RedirectToAction("Detail", new { id = id });
        }
    }
}
