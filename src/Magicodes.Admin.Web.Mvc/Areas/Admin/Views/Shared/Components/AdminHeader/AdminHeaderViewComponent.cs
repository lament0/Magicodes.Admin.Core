﻿using System.Threading.Tasks;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using Magicodes.Admin.Web.Areas.Admin.Models.Layout;
using Magicodes.Admin.Web.Session;

namespace Magicodes.Admin.Web.Areas.Admin.Views.Shared.Components.AdminHeader
{
    public class AdminHeaderViewComponent : ViewComponent
    {
        private readonly ILanguageManager _languageManager;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly IPerRequestSessionCache _sessionCache;
        private readonly IAbpSession _abpSession;

        public AdminHeaderViewComponent(
            IMultiTenancyConfig multiTenancyConfig, 
            IAbpSession abpSession,
            ILanguageManager languageManager, 
            IPerRequestSessionCache sessionCache)
        {
            _multiTenancyConfig = multiTenancyConfig;
            _abpSession = abpSession;
            _languageManager = languageManager;
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var headerModel = new HeaderViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync(),
                Languages = _languageManager.GetLanguages(),
                CurrentLanguage = _languageManager.CurrentLanguage,
                IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled,
                IsImpersonatedLogin = _abpSession.ImpersonatorUserId.HasValue
            };

            return View(headerModel);
        }
    }
}
