using System;
using LuwAdmin.Web.Models;

namespace LuwAdmin.Web.DomainServices
{
    public class ExpirationService
    {
        public bool IsExpiring(PersonType personType, DateTime whenExpires, DateTime? today = null)
        {
            today = today ?? DateTime.Now;

            var whenStartExpiration = today.Value.Date.AddDays(-1 * personType.StartSendingRenewalDays);
            var whenEndExpiration = today.Value.Date.AddDays(personType.StopSendingRenewalDays);
            if (whenExpires < whenStartExpiration || whenExpires > whenEndExpiration)
            {
                return false;
            }

            return true;
        }

        public bool HasExpired(DateTime whenExpires, DateTime? today = null)
        {
            today = today ?? DateTime.Now;

            var whenEndExpiration = today.Value.Date;
            if (whenExpires < whenEndExpiration)
            {
                return true;
            }
            return false;
        }

        public int DaysToExpiration(DateTime whenExpires, DateTime? today = null)
        {
            today = today ?? DateTime.Now;

            if (today.Value.Date <= whenExpires.Date)
            {
                return whenExpires.Date.Subtract(today.Value.Date).Days;
            }
            return today.Value.Date.Subtract(whenExpires.Date).Days * -1;
        }
    }
}
