using System;
using System.Collections.Generic;
using commercetools.Sdk.Client;
using commercetools.Sdk.Domain;
using commercetools.Sdk.Domain.Channels;

namespace commercetools.Sdk.HttpApi.IntegrationTests.Channels
{
    public class ChannelFixture : ClientFixture, IDisposable 
    {
        public List<Channel> Channels { get; }
        
        public ChannelFixture() : base()
        {
            this.Channels = new List<Channel>();
        }
        
        public void Dispose()
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            this.Channels.Reverse();
            foreach (Channel channel in this.Channels)
            {
                Channel deletedEntry = commerceToolsClient
                    .ExecuteAsync(new DeleteByIdCommand<Channel>(new Guid(channel.Id), channel.Version)).Result;
            }
        }
        public ChannelDraft GetChannelDraft()
        {
            ChannelDraft channelDraft = new ChannelDraft();
            channelDraft.Key = this.RandomString(10);
            return channelDraft;
        }

        public Channel CreateChannel()
        {
            return this.CreateChannel(this.GetChannelDraft());
        }

        public Channel CreateChannel(ChannelDraft channelDraft)
        {
            IClient commerceToolsClient = this.GetService<IClient>();
            Channel channel = commerceToolsClient.ExecuteAsync(new CreateCommand<Channel>(channelDraft)).Result;
            return channel;
        }
    }
}