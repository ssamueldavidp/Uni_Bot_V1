using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CollegeBot.Dialogs.CreateCita
{
    public class CreateCitaDialog: ComponentDialog
    {
        public CreateCitaDialog()
        {
            //es la array que contiene los metodos que se ejecutan secuencialmente
            var waterfallStep = new WaterfallStep[] 
            {
                SetPhone,
                SetFullName,
                SetEmail,
                SetDate,
                SetTime,
                Confirmation,
                FinalProcess 
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog),waterfallStep));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
        
        }



        private async  Task<DialogTurnResult> SetPhone(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(
                nameof(TextPrompt),
                new PromptOptions { Prompt = MessageFactory.Text("Para inciar con el agendamiento por favor ingresa tu numero de teléfono ")},cancellationToken
                );
        }

        private async Task<DialogTurnResult> SetFullName(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //Se optiene el Número que ingreso el usuario
            var userPhone = stepContext.Context.Activity.Text;

            return await stepContext.PromptAsync(
                nameof(TextPrompt),
                new PromptOptions { Prompt = MessageFactory.Text("Ingrese su nombre completo")}, cancellationToken
                );
        }

        private async Task<DialogTurnResult> SetEmail(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //Se optiene el nombre que ingreso el usuario
            var fullName = stepContext.Context.Activity.Text;

            return await stepContext.PromptAsync(
                
                nameof(TextPrompt),
                new PromptOptions { Prompt = MessageFactory.Text("Ingrese su correo: ")}, cancellationToken
                );
        }

        private async Task<DialogTurnResult> SetDate(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var email = stepContext.Context.Activity.Text;

            string text = $"Ingrese La fecha en la que le gustaria agendar la cita presencial" +
                $"{ Environment.NewLine}Recuerde que el formato para ingresar la fecha es el siguiente: dd / mm / yyyy";
            return await stepContext.PromptAsync(

                nameof(TextPrompt),
                new PromptOptions { Prompt = MessageFactory.Text(text) }, cancellationToken
            );
        }

        private async Task<DialogTurnResult> SetTime(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var dateCita = stepContext.Context.Activity.Text;

            return await stepContext.PromptAsync(
                 nameof(TextPrompt),
                 new PromptOptions { Prompt = CreateButtonsTime()}, cancellationToken
                );
        }


        private async Task<DialogTurnResult> Confirmation(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var timeCita = stepContext.Context.Activity.Text;
            return await stepContext.PromptAsync(
                nameof(TextPrompt),
                new PromptOptions { Prompt = CreateButtonConfirmation() }, cancellationToken
                );
        }

        

        private async Task<DialogTurnResult> FinalProcess(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var confirmationUser = stepContext.Context.Activity.Text;

            if (confirmationUser.ToLower().Equals("No"))
            {
                //simulación creacion de cita por falta de presupuesto no se usa una db 
                await stepContext.Context.SendActivityAsync("No creare la cita", cancellationToken: cancellationToken);
            }
            else {
                await stepContext.Context.SendActivityAsync("Su Cita fue creada", cancellationToken: cancellationToken
                    );
            }

            //continue con el siguiemte dialog
            return await stepContext.ContinueDialogAsync(cancellationToken: cancellationToken);
        }

        private Activity CreateButtonsTime()
        {
            var reply = MessageFactory.Text("Seleccione la hora");

            reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction() { Title = "8", Value = "8", Type = ActionTypes.ImBack },
                    new CardAction() { Title = "9", Value = "9", Type = ActionTypes.ImBack },
                    new CardAction() { Title = "10", Value = "10", Type = ActionTypes.ImBack },
                    new CardAction() { Title = "11", Value = "11", Type = ActionTypes.ImBack },
                    new CardAction() { Title = "12", Value = "12", Type = ActionTypes.ImBack },
                    new CardAction() { Title = "13", Value = "13", Type = ActionTypes.ImBack },
                    new CardAction() { Title = "14", Value = "14", Type = ActionTypes.ImBack },
                    new CardAction() { Title = "15", Value = "15", Type = ActionTypes.ImBack },
                    new CardAction() { Title = "16", Value = "16", Type = ActionTypes.ImBack },
                    new CardAction() { Title = "17", Value = "17", Type = ActionTypes.ImBack },
                    new CardAction() { Title = "18", Value = "18", Type = ActionTypes.ImBack },
                    new CardAction() { Title = "19", Value = "19", Type = ActionTypes.ImBack },
                    new CardAction() { Title = "20", Value = "20", Type = ActionTypes.ImBack },
                    new CardAction() { Title = "21", Value = "21", Type = ActionTypes.ImBack },
                    }
                };
            return reply as Activity;
        }
        private Activity CreateButtonConfirmation()
        {
            var Reply = MessageFactory.Text("Al seleccinar el boton se confirmara el horario establecido para asistir a la cita ");
            Reply.SuggestedActions = new SuggestedActions()
            {
                Actions = new List<CardAction>()
                {
                    new CardAction(){ Title = "Si", Value = "Si", Type = ActionTypes.ImBack},
                    new CardAction(){ Title = "No", Value = "No", Type = ActionTypes.ImBack},
                }
            };
            return Reply as Activity;
        }
    }
}
