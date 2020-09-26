namespace BlazorCommunity.DTO
{
    public class ChangePwdDto
    {
        public string Account { get; set; }
        public string OldPwd { get; set; }
        public string NewPwd { get; set; }
    }
}