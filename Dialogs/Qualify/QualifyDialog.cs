using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CollegeBot.Dialogs.Qualify
{
    public class QualifyDialog: ComponentDialog
    {
        public QualifyDialog()
        { 

            //se ejecutaran secuencialmente
            var waterfallSteps = new WaterfallStep[]
            {
                ToShowButton,
                Validateoption
            };
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
        }

        //Validar la entrada del usuario
        private async Task<DialogTurnResult> Validateoption(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var options = stepContext.Context.Activity.Text;
            await stepContext.Context.SendActivityAsync($"Gracias, su selección fue:  {options}", cancellationToken: cancellationToken);
            await Task.Delay(1000);
            await stepContext.Context.SendActivityAsync("¿Quieres que te ayude en algo más?", cancellationToken: cancellationToken);

            return await stepContext.ContinueDialogAsync(cancellationToken: cancellationToken);
        }




        //Construccion de botones 
        private Activity CreateButtonsQualify()
        {
            var reply = MessageFactory.Text("Por favor introduzca la calificación, de acuerdo con el nivel de satisfacción del bot.");
            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction(){ Title = "1⭐", Value = "1⭐", Type = ActionTypes.ImBack},
                    new CardAction(){ Title = "2⭐", Value = "2⭐", Type = ActionTypes.ImBack},
                    new CardAction(){ Title = "3⭐", Value = "3⭐", Type = ActionTypes.ImBack},
                    new CardAction(){ Title = "4⭐", Value = "4⭐", Type = ActionTypes.ImBack},
                    new CardAction(){ Title = "5⭐", Value = "5⭐", Type = ActionTypes.ImBack},


                }
            };
            return reply as Activity;
        }

        private async Task<DialogTurnResult> ToShowButton(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(
                 nameof(TextPrompt),
                 new PromptOptions
                 {
                     Prompt = CreateButtonsQualify()
                 },
                 cancellationToken
                 );
        }
    }
}
