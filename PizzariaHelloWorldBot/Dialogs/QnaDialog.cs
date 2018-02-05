using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace PizzariaHelloWorldBot.Dialogs
{
    [Serializable]
    public class QnaDialog : QnAMakerDialog
    {
        public QnaDialog() : base(new QnAMakerService(new QnAMakerAttribute(System.Configuration.ConfigurationSettings.AppSettings["QnaSubscriptionKey"],System.Configuration.ConfigurationSettings.AppSettings["QnaKnowLedgebasesKey"], "Não encontramos uma resposta para sua dúvida...",0.5)))
        {

        }

        protected override async Task RespondFromQnAMakerResultAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result)
        {
            var primeiraResposta = result.Answers.First().Answer;

            var resposta = ((Activity)context.Activity).CreateReply();

            var dadosResposta = primeiraResposta.Split('|');

            if (dadosResposta.Length == 1)
            {
                await context.PostAsync(primeiraResposta);
                return;
            }

            var heroCard = new HeroCard()
            {
                Title = dadosResposta[0],
                Subtitle = dadosResposta[1],
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.OpenUrl,"Compre Agora", dadosResposta[2])
                },
                Images = new List<CardImage>()
                {
                    new CardImage(dadosResposta[3])
                }
            };

            resposta.Attachments.Add(heroCard.ToAttachment());

            await context.PostAsync(resposta);   
        }
    }
}