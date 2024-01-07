using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace Diploma_DataAccess.DTOs
{
    public class TelegramSettings
    {
        public string BotToken { get; set; }
        public string CHAT_ID { get; set; }
    }
}
