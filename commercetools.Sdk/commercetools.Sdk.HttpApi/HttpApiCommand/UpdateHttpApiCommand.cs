﻿using commercetools.Sdk.Client;
using System.Net.Http;

namespace commercetools.Sdk.HttpApi
{
    public class UpdateHttpApiCommand<T> : IHttpApiCommand<UpdateCommand<T>, T>
    {
        private UpdateCommand<T> command;
        private readonly UpdateRequestMessageBuilder requestBuilder;

        public HttpRequestMessage HttpRequestMessage
        {
            get
            {
                return this.requestBuilder.GetRequestMessage(command);
            }
        }

        public UpdateHttpApiCommand(UpdateCommand<T> command, IRequestMessageBuilderFactory requestMessageBuilderFactory)
        {
            this.command = command;
            this.requestBuilder = requestMessageBuilderFactory.GetRequestMessageBuilder<UpdateRequestMessageBuilder>();
        }
    }
}