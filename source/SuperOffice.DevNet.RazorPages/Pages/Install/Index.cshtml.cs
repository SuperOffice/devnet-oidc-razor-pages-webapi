using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuperOffice.DevNet.Asp.Net.RazorPages.Data;
using SuperOffice.DevNet.Asp.Net.RazorPages.Models;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Pages.Install
{
    public class IndexModel : PageModel
    {
        private readonly ProvisioningDbContext _provDbContext;

        public IndexModel(ProvisioningDbContext provisioningDbContext)
        {
            this._provDbContext = provisioningDbContext;
        }
        public async Task OnGetAsync()
        {
            //await _provDbContext.GetAllWebPanels();

            //var newPanel = await _provDbContext.SaveAppWebPanel(new WebPanelEntityModel
            //{
            //    Name = "DeveloperCommunity",
            //    WindowName = "developercommunity",
            //    Deleted = false,
            //    ShowInAddressBar = false,
            //    ShowInMenuBar = false,
            //    ShowInStatusBar = false,
            //    ShowInToolBar = false,
            //    OnCentral = true,
            //    OnSalesMarketingPocket = false,
            //    OnSalesMarketingWeb = true,
            //    OnSatellite = false,
            //    OnTravel = false,
            //    Tooltip = "Testing Provisioning",
            //    Url = "https://community.superoffice.com/en/developer/create-apps/",
            //    UrlEncoding = Models.Enums.UrlEncodingModel.None,
            //    VisibleIn = VisibleInHelper.GeneralUI.SuperOfficeLogo
            //});

            return;
        }
    }
}