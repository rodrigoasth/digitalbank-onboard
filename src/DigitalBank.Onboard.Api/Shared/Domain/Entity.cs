using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalBank.Onboard.Api.Shared.Domain
{
    public abstract class Entity
    {
        protected NotificationContext NotificationContext { get; private set; }

        public Entity()
        {
            NotificationContext = new NotificationContext();
        }

        public bool RulesIsBroken()
        {
            return NotificationContext.HasNotifications;
        }

        public string GetBrokenRules()
        {
            return string.Join(" | ", NotificationContext.Notifications.Select(x => x.Message));
        }
    }    
}