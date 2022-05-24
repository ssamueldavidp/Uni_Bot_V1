using CollegeBot.common.Cards;
using CollegeBot.Dialogs.CreateCita;
using CollegeBot.Dialogs.Qualify;
using CollegeBot.Infraestructure.Luis;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CollegeBot.Dialogs
{
    public class RootDialog: ComponentDialog
    {
        private readonly ILuisService _luisService;

        public RootDialog(ILuisService luisService) 
        {
            _luisService = luisService;
            //Array paera contner metodos que se ejecutan secuencialmente
            var waterfallSteps = new WaterfallStep[]
            {
                InitialProcess,
                FinalProcess
            };

            AddDialog(new CreateCitaDialog());
            AddDialog(new QualifyDialog());
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));           
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> InitialProcess(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var luisResult = await _luisService._luisRecognizer.RecognizeAsync(stepContext.Context, cancellationToken);

            return await ManageIntentions(stepContext, luisResult, cancellationToken);
        }

        private async Task<DialogTurnResult> ManageIntentions(WaterfallStepContext stepContext, Microsoft.Bot.Builder.RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            var topIntent = luisResult.GetTopScoringIntent();
            switch (topIntent.intent) 
            {
                case "Saludo":
                    await IntentSaludo(stepContext, luisResult, cancellationToken);
                    break;
                case "Inscripciones":
                    await IntentInscripciones(stepContext, luisResult, cancellationToken);
                    break;
                case "horario":
                    await IntentHorario(stepContext, luisResult, cancellationToken);
                    break;
                case "contacto":
                    await IntentContacto(stepContext, luisResult, cancellationToken);
                    break;
                case "sedes":
                    await IntentSedes(stepContext, luisResult, cancellationToken);
                    break;
                case "carreras":
                    await IntentCarreras(stepContext, luisResult, cancellationToken);
                    break;
                case "Despedida":
                    await IntentDespedida(stepContext, luisResult, cancellationToken);
                    break;
                case "Opciones":
                    await IntentOpciones(stepContext, luisResult, cancellationToken);
                    break;
                case "Calificar":
                    return await IntentCalificar(stepContext, luisResult, cancellationToken);
                    break;
                case "Cita":
                    return await IntentCita(stepContext, luisResult, cancellationToken);
                    break;
                case "None":
                    await IntentNone(stepContext, luisResult, cancellationToken);
                    break;
                default:
                    break;

            }
            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }




        #region IntentsLuis


        private async Task<DialogTurnResult> IntentCita(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            return await stepContext.BeginDialogAsync(nameof(CreateCitaDialog), cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> IntentCalificar(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            return await stepContext.BeginDialogAsync(nameof(QualifyDialog), cancellationToken: cancellationToken);

        }


        private async Task IntentOpciones(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("Este es el menu de opciones", cancellationToken: cancellationToken);
            await MainOptions.ToShow(stepContext, cancellationToken);

        }

        private async Task IntentNone(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("🤔 Perdón, en este momento no tengo una respuesta para tu pregunta, estoy aprendiendo cada día más para darte solución a todas tus solicitudes ", cancellationToken: cancellationToken);
        }

        private async Task IntentDespedida(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("🤖👋 fue todo un placer hablar contigo estare aquí para ti 24/7 respondiendo todas tus dudas bye!", cancellationToken: cancellationToken);
        }

        private async Task IntentCarreras(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync($"Actualmente, contamos con 5 Facultades, la cuales han formado grandes profesionales 🎓 ¿Quieres ser parte? {Environment.NewLine}" +
                $"Facultad de Ingeniería {Environment.NewLine} " +
                $"Facultad de Ciencias Empresariales {Environment.NewLine}" +
                $"Facultad de Ciencias Sociales y de la Educación {Environment.NewLine}" +
                $"Facultad de Contaduría y Finanzas Internacionales {Environment.NewLine}" +
                $"Facultad de Comunicación {Environment.NewLine}" +
                $"Si quieres conocer las carreras que estas facultades ofrecen para ti puedes ingresar al siguiente enlace: https://ucompensar.edu.co/facultades/{Environment.NewLine} ", cancellationToken: cancellationToken);
        }

        private  async Task IntentSedes(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync($"Estas son nuestras sedes actuales {Environment.NewLine}" +
                $"BOGOTÁ Sede Teusaquillo: Avenida(Calle) 32 No. 17 – 30 {Environment.NewLine}"  +
                $"VIRTUAL Línea nacional: 01 8000 110 666 Correo electrónico: aspirantes@ucompensar.edu.co" , cancellationToken: cancellationToken);
        }

        private async Task IntentContacto(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            string phoneOptions = $"Te puedes comunircar con nostros atravez de las sigientes lienas telefonicas: {Environment.NewLine}"+ 
                $"📞 +57 3174659663 Samuel Rodriguez{Environment.NewLine} 📞 +57 3012483629 Andres Ortega {Environment.NewLine} 📞 +57 3227718552 Sebastian Parra ";

            string addressUni = $"puedes acercarte a nuestros puntos fisicos en los hoarios establecidos Dirección: {Environment.NewLine}" +
                $"📍 Ac. 32 #16-64, Teusaquillo, Bogotá{Environment.NewLine}";

            await stepContext.Context.SendActivityAsync(phoneOptions, cancellationToken: cancellationToken);
            //Esperar 1 seg para dar a otra resuesta 
            await Task.Delay(1000);
            await stepContext.Context.SendActivityAsync(addressUni, cancellationToken: cancellationToken);
            await Task.Delay(1000);
            await stepContext.Context.SendActivityAsync("¿Quieres que te ayude en algo más?", cancellationToken: cancellationToken);
        }

        private async Task IntentHorario(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("Puedes acercarte a nuestros puntos fisicos desde las 8:00 am 9:00 pm" +
                "Si quieres llamarnos nuestros horarios de atecion en la mañana son de 8:00 am - 12:00 y en las tardes desde la 1:00 pm - pm 9:30 pm" +
                "Si tienes dudas recuerda que estoy disponible las 24/7 🤖 ", cancellationToken: cancellationToken);

        }

        private async Task IntentInscripciones(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync($"Requisitos de Admisión 📅 {Environment.NewLine} " +
                $"🧾 Diligencia el formulario de inscripción {Environment.NewLine} " +
                $"✉ Reúne la documentación requerida y adjúntala a la plataforma de inscripción o envíala al correo aspirantes@ucompensar.edu.co {Environment.NewLine} " +
                $"💻 puesdes ampliar esta información en el seguiente enlace: https://ucompensar.edu.co/inscripciones/", cancellationToken: cancellationToken);
        }

        private async Task IntentSaludo(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("Hola 👋🤖 me presento nuevamente Soy el Chatbot de la universidad 🤖, para la universidad es un placer saludarte," +
                " recuerda que solucionaré todas las preguntas que tienes para mí.", cancellationToken: cancellationToken);

        }
        #endregion
        private async Task<DialogTurnResult> FinalProcess(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

        
    }

}
