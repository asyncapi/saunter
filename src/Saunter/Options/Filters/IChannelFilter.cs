using LEGO.AsyncAPI.Models;

public interface IChannelFilter
{
    void Apply(AsyncApiChannel channel, ChannelFilterContext context);
}
