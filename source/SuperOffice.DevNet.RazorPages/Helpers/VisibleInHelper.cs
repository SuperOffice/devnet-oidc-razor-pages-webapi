using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using SuperOffice.DevNet.Asp.Net.RazorPages.Models.Enums;

namespace SuperOffice.DevNet.Asp.Net.RazorPages
{
    public static class VisibleInHelper
    {
        public static class Dialogs
        {
            public static VisibleInModel Document
            {
                get { return VisibleInModel.DocumentDialog; }
            }

            public static VisibleInModel Followup
            {
                get { return VisibleInModel.ActivityDialog; }
            }

            public static VisibleInModel Product
            {
                get { return VisibleInModel.QuoteLineDialog; }
            }

            public static VisibleInModel Quote
            {
                get { return VisibleInModel.QuoteDialog; }
            }
        }

        public static class EntityArchiveTabs
        {
            /// <summary>
            /// Visible on the Company archive tabs.
            /// </summary>
            public static VisibleInModel Company
            {
                get { return VisibleInModel.ContactArchive; }
            }

            /// <summary>
            /// Visible on the Contact (person) archive tabs.
            /// </summary>
            public static VisibleInModel Contact
            {
                get { return VisibleInModel.PersonArchive; }
            }

            /// <summary>
            /// Visible on the project archive tabs.
            /// </summary>
            public static VisibleInModel Project
            {
                get { return VisibleInModel.ProjectArchive; }
            }

            /// <summary>
            /// Visible on the sale archive tabs.
            /// </summary>
            public static VisibleInModel Sale
            {
                get { return VisibleInModel.SaleArchive; }
            }
        }

        public static class EntityCards
        {
            /// <summary>
            /// Visible on the Company card.
            /// </summary>
            public static VisibleInModel Company
            {
                get { return VisibleInModel.ContactCard; }
            }

            /// <summary>
            /// Visible on the Contact (person) card.
            /// </summary>
            public static VisibleInModel Contact
            {
                get { return VisibleInModel.PersonCard; }
            }

            /// <summary>
            /// Visible on the project card.
            /// </summary>
            public static VisibleInModel Project
            {
                get { return VisibleInModel.ProjectCard; }
            }

            /// <summary>
            /// Visible on the sale card.
            /// </summary>
            public static VisibleInModel Sale
            {
                get { return VisibleInModel.SaleCard; }
            }

            /// <summary>
            /// Visible on the selection card.
            /// </summary>
            public static VisibleInModel Selection
            {
                get { return VisibleInModel.SelectionCard; }
            }
        }

        public static class MenuItems
        {
            /// <summary>
            /// Visible under the Hamburger menu -> Other applications list.
            /// </summary>
            public static VisibleInModel OtherApplications
            {
                get { return VisibleInModel.ViewMenu; }
            }

            /// <summary>
            /// Visible under Tools navigator button and the Hamburger menu -> Other applications list.
            /// </summary>
            public static VisibleInModel ToolboxListItem
            {
                get { return VisibleInModel.ToolboxMenu; }
            }
        }

        public static class NavigatorButtons
        {
            public static VisibleInModel NavigatorButton
            {
                get { return VisibleInModel.NavigatorButton; }
            }

            public static VisibleInModel ToolboxListItem
            {
                get { return VisibleInModel.ToolboxMenu; }
            }
        }

        public static class TaskButtons
        {
            /// <summary>
            /// Visible in the Contact (person) task button menu list.
            /// </summary>
            public static VisibleInModel Contact
            {
                get { return VisibleInModel.PersonDialogTask; }
            }

            /// <summary>
            /// Visible in the Followup (appointment) task button menu list.
            /// </summary>
            public static VisibleInModel Followup
            {
                get { return VisibleInModel.AppointmentDialogTask; }
            }
        }

        public static class GeneralUI
        {
            /// <summary>
            /// Visible in the Contact (person) task button menu list.
            /// </summary>
            public static VisibleInModel MiniCard
            {
                get { return VisibleInModel.CompanyMinicard; }
            }

            /// <summary>
            /// Visible in the Followup (appointment) task button menu list.
            /// </summary>
            public static VisibleInModel SuperOfficeLogo
            {
                get { return VisibleInModel.BrowserPanel; }
            }
        }

        public static List<SelectListItem> GetSelectListItems()
        {
            var archives = new SelectListGroup { Name = "Archives" };
            var buttons = new SelectListGroup { Name = "Button" };
            var cards = new SelectListGroup { Name = "Cards" };
            var dialogs = new SelectListGroup { Name = "Dialogs" };
            var lists = new SelectListGroup { Name = "List" };
            var otherUI = new SelectListGroup { Name = "Other" };
            var taskButtons = new SelectListGroup { Name = "Tasks" };

            return new List<SelectListItem>
            {

                new SelectListItem{ Text = nameof(VisibleInHelper.EntityArchiveTabs.Company)       , Value = VisibleInHelper.EntityArchiveTabs.Company.ToString()       , Group = archives },
                new SelectListItem{ Text = nameof(VisibleInHelper.EntityArchiveTabs.Contact)       , Value = VisibleInHelper.EntityArchiveTabs.Contact.ToString()       , Group = archives },
                new SelectListItem{ Text = nameof(VisibleInHelper.EntityArchiveTabs.Project)       , Value = VisibleInHelper.EntityArchiveTabs.Project.ToString()       , Group = archives },
                new SelectListItem{ Text = nameof(VisibleInHelper.EntityArchiveTabs.Sale)          , Value = VisibleInHelper.EntityArchiveTabs.Sale.ToString()          , Group = archives },

                new SelectListItem{ Text = nameof(VisibleInHelper.NavigatorButtons.NavigatorButton), Value = VisibleInHelper.NavigatorButtons.NavigatorButton.ToString(), Group = buttons },
                new SelectListItem{ Text = nameof(VisibleInHelper.NavigatorButtons.ToolboxListItem), Value = VisibleInHelper.NavigatorButtons.ToolboxListItem.ToString(), Group = buttons },

                new SelectListItem{ Text = nameof(VisibleInHelper.EntityCards.Company)             , Value = VisibleInHelper.EntityCards.Company.ToString()             , Group = cards },
                new SelectListItem{ Text = nameof(VisibleInHelper.EntityCards.Contact)             , Value = VisibleInHelper.EntityCards.Contact.ToString()             , Group = cards },
                new SelectListItem{ Text = nameof(VisibleInHelper.EntityCards.Project)             , Value = VisibleInHelper.EntityCards.Project.ToString()             , Group = cards },
                new SelectListItem{ Text = nameof(VisibleInHelper.EntityCards.Sale)                , Value = VisibleInHelper.EntityCards.Sale.ToString()                , Group = cards },
                new SelectListItem{ Text = nameof(VisibleInHelper.EntityCards.Selection)           , Value = VisibleInHelper.EntityCards.Selection.ToString()           , Group = cards },

                new SelectListItem{ Text = nameof(VisibleInHelper.Dialogs.Document)                , Value = VisibleInHelper.Dialogs.Document.ToString()                , Group = dialogs },
                new SelectListItem{ Text = nameof(VisibleInHelper.Dialogs.Followup)                , Value = VisibleInHelper.Dialogs.Followup.ToString()                , Group = dialogs },
                new SelectListItem{ Text = nameof(VisibleInHelper.Dialogs.Product)                 , Value = VisibleInHelper.Dialogs.Product.ToString()                 , Group = dialogs },
                new SelectListItem{ Text = nameof(VisibleInHelper.Dialogs.Quote)                   , Value = VisibleInHelper.Dialogs.Quote.ToString()                   , Group = dialogs },

                new SelectListItem{ Text = nameof(VisibleInHelper.MenuItems.OtherApplications)     , Value = VisibleInHelper.MenuItems.OtherApplications.ToString()     , Group = lists },
                new SelectListItem{ Text = nameof(VisibleInHelper.MenuItems.ToolboxListItem)       , Value = VisibleInHelper.MenuItems.ToolboxListItem.ToString()       , Group = lists },

                new SelectListItem{ Text = nameof(VisibleInHelper.TaskButtons.Contact)             , Value = VisibleInHelper.TaskButtons.Contact.ToString()             , Group = taskButtons },
                new SelectListItem{ Text = nameof(VisibleInHelper.TaskButtons.Followup)            , Value = VisibleInHelper.TaskButtons.Followup.ToString()            , Group = taskButtons },

                new SelectListItem{ Text = nameof(VisibleInHelper.GeneralUI.MiniCard)              , Value = VisibleInHelper.GeneralUI.MiniCard.ToString()              , Group = otherUI },
                new SelectListItem{ Text = nameof(VisibleInHelper.GeneralUI.SuperOfficeLogo)       , Value = VisibleInHelper.GeneralUI.SuperOfficeLogo.ToString()       , Group = otherUI }
            };
        }


    }
}
