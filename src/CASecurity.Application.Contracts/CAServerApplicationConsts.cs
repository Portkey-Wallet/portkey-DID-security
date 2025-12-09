namespace CASecurity;

public class CASecurityApplicationConsts
{
    public const string MessageStreamName = "CASecurity";
    
    public const string EmailRegex = @"[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?";
    public const string PhoneRegex = @"^1[3456789]\d{9}$";
}