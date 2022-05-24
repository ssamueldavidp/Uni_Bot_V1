using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CollegeBot.common.Cards
{
    public class MainOptions
    {
        public static async Task ToShow(DialogContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(activity: CreateCarousel(), cancellationToken);
        }
        private static Activity CreateCarousel()
        {
            var cardCita = new HeroCard
            {
                Title = "Turno ",
                Subtitle = "Opciones",
                Images = new List<CardImage> { new CardImage("https://chatuniversidadstorage.blob.core.windows.net/images/3896559.jpg") },
                Buttons = new List<CardAction>() 
                {
                    new CardAction(){Title = "crea cita presencial", Value = "crea cita presencial", Type = ActionTypes.ImBack},
                   

                }

            };
            var cardInfo = new HeroCard
            {
                Title = "Contacto ",
                Subtitle = "Esta es nuestra informacion de contacto",
                Images = new List<CardImage> { new CardImage("https://chatuniversidadstorage.blob.core.windows.net/images/2453787.jpg") },
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title = "Centro de contacto", Value = "Centro de contacto", Type = ActionTypes.ImBack},
                    new CardAction(){Title = "Sitio web", Value = "https://ucompensar.edu.co/", Type = ActionTypes.openApp},

                }

            };
            var cardCalificar = new HeroCard
            {
                Title = "Calificacion ",
                Subtitle = "Opciones",
                Images = new List<CardImage> { new CardImage("https://chatuniversidadstorage.blob.core.windows.net/images/Mi proyecto.jpg") },
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title = "Calificar Bot", Value = "Calificar Bot", Type = ActionTypes.ImBack}

                }

            };

            var optionsAttachments = new List<Attachment>() 
            { 
                cardCita.ToAttachment(),
                cardInfo.ToAttachment(),
                cardCalificar.ToAttachment(),
            };
            //
            var reply = MessageFactory.Attachment(optionsAttachments);
            // tipo de trjeta
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            return reply as Activity;
        }

    }
}
