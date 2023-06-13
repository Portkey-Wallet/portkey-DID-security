namespace CAVerifierServer.IpWhiteList;

public class AddUserIpToWhiteListRequestDto
{
    public string Path { get; set; }

    public string ParamsJsonObject { get; set; }

    public string UserIp { get; set; }
}