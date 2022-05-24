using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace CollegeBot.Infraestructure.Luis
{
    public class LuisService: ILuisService
    {
        public LuisRecognizer _luisRecognizer { get; set; }

        public LuisService(IConfiguration configuration)
        {
            var LuisApplication = new LuisApplication(
                
                configuration["LuisAppId"],
                configuration["LuisApiKey"],
                configuration["LuisHostName"]
             );
            var recognizerOptions = new LuisRecognizerOptionsV3(LuisApplication) { 
                PredictionOptions = new Microsoft.Bot.Builder.AI.LuisV3.LuisPredictionOptions()
                {
                    IncludeInstanceData = true
                }

            };
            _luisRecognizer = new LuisRecognizer(recognizerOptions);
               
        }
    }
}
