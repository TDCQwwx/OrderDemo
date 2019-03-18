using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OrderDemo.Common
{
    public class HTTPHelper
    {
        #region Fields
        private static HttpClient _httpClient;
        private const string url = "http://192.168.1.105:9000";
        private const string urlRegister = url + "/PC/RegisterUserByPC";
        private const string urlLoginUserName = url + "/PC/LoginByUserName";
        private const string urlLoginPhoneNumber = url + "/PC/LoginByPhoneNumber";
        private const string urlExit = url + "/PC/ExitByPC";
        private const string urlSubmitOrder = url + "/PC/AddOrder";
        private const string urlQueryOrder = url + "/PC/GetCurrentCustomerOrderStates";
        private const string urlTakeOrder = url + "/PC/AddDeliveryOrder";
        #endregion

        #region Methods
        #region 连接
        private static void Connect()
        {
            _httpClient = new HttpClient();
        }
        #endregion
        #region 注册
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="password">密码</param>
        /// <returns>注册结果</returns>
        public async static Task<ResultData> Register(string userName,string phoneNumber,string password)
        {
            if (_httpClient == null) Connect();
            var dataRegister = new UsersRegister() { UserName = userName, PhoneNumber = phoneNumber, Password = long.Parse(password) };
            string jsonData = JsonConvert.SerializeObject(dataRegister);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(urlRegister, content);
            var responseString = await response.Content.ReadAsStringAsync();
            ResultData code = JsonConvert.DeserializeObject<ResultData>(responseString);
            return code;
        }
        #endregion
        #region 登录
        
        public async static Task<ResultData> Login(LoginModeEnum loginModeEnum,string userNameorPhoneNumber,string password)
        {

            if (_httpClient == null) Connect();

            string url = null;
            string jsonData = null;

            switch (loginModeEnum)
            {
                case LoginModeEnum.LoginbyUserName:
                    var data1 = new LoginUserNameDto() { UserName = userNameorPhoneNumber, Password = password };
                    jsonData = JsonConvert.SerializeObject(data1);
                    url = urlLoginUserName;
                    break;
                case LoginModeEnum.LoginbyPhoneNumber:
                    var data2 = new LoginPhoneNumberDto() { PhoneNumber = userNameorPhoneNumber, Password = password };
                    jsonData = JsonConvert.SerializeObject(data2);
                    url = urlLoginPhoneNumber;
                    break;
                default:
                    break;
            }

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();
            ResultData code = JsonConvert.DeserializeObject<ResultData>(responseString);

            return code;
        }
        #endregion
        #region 退出
        public async static Task<ResultData> Exit()
        {
            if (_httpClient == null) Connect();

            HttpResponseMessage response = await _httpClient.GetAsync(urlExit);
            var responseString = await response.Content.ReadAsStringAsync();
            ResultData code = JsonConvert.DeserializeObject<ResultData>(responseString);

            return code;
        }
        #endregion
        #region 订单提交
        public async static Task<AddOrderResult> SubmitOrder(string printContent,List<Coordinate> coordinates)
        {
            if (_httpClient == null) Connect();

            var data = new OrderParameterDto() { PrintContent = printContent, Priority = 0, Coordinates = coordinates };
            string jsonData = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(urlSubmitOrder, content);
            var responseString = await response.Content.ReadAsStringAsync();
            AddOrderResult code = JsonConvert.DeserializeObject<AddOrderResult>(responseString);

            return code;
        }
        #endregion
        #region 订单查询
        public async static Task<OrderStateListDto> QueryOrder()
        {
            if (_httpClient == null) Connect();

            HttpResponseMessage response = await _httpClient.GetAsync(urlQueryOrder);
            var responseString = await response.Content.ReadAsStringAsync();
            OrderStateListDto codes = JsonConvert.DeserializeObject<OrderStateListDto>(responseString);

            return codes;
        }
        #endregion
        #region 取件
        public async static Task<ResultData> TakeOrder(string orderID)
        {
            if (_httpClient == null) Connect();

            var data = new DeliveryOrderMessage() { OrderID = orderID };
            string jsonData = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(urlTakeOrder, content);
            var responseString = await response.Content.ReadAsStringAsync();
            ResultData code = JsonConvert.DeserializeObject<ResultData>(responseString);

            return code;
        }
        #endregion
        #endregion
    }
    #region APIHelperClass
    //上传参数
    //HTTP POST
    #region 注册——上传参数
    public class UsersRegister
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public long Password { get; set; }
    }
    #endregion
    #region 登录——上传参数
    //用户名登录
    public class LoginUserNameDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    //手机号登录
    public class LoginPhoneNumberDto
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
    #endregion
    #region 订单提交——上传参数
    public class OrderParameterDto
    {
        public string PrintContent { get; set; }
        public List<Coordinate> Coordinates { get; set; }
        public int Priority { get; set; }
    }
    public class Coordinate
    {
        public double X { get; set; }
        public double Z { get; set; }
    }
    #endregion
    #region 取件——上传参数
    public class DeliveryOrderMessage
    {
        public string OrderID { get; set; }
    }
    #endregion
    //HTTP GET
    #region 退出——无上传参数

    #endregion
    #region 订单查询——无上传参数

    #endregion
    //返回参数
    #region 注册||登录||退出||取件——返回参数
    public class ResultData
    {
        public string Result { get; set; }
        public ErrorCode Code { get; set; }
    }
    public enum ErrorCode
    {
        OK = 0,
        UserNotLogined,
        WrongParameter,
        UserNameExisted,
        PhoneNumberExisted,
        Exception
    }
    #endregion
    #region 订单提交——返回参数
    public class AddOrderResult
    {
        public string Result { get; set; }
        public string OrderID { get; set; }
    }
    #endregion
    #region 订单查询——返回参数
    public class OrderStateListDto
    {
        public List<OrderStateDto> States { get; set; }
    }
    public class OrderStateDto
    {
        public string OrderID { get; set; }
        public string OrderStartTime { get; set; }
        public string OrderFinishTime { get; set; }
        public string OrderDeliveryTime { get; set; }
        public string CurrentState { get; set; }
        public string ProcessingName { get; set; }
        public string ProcessingState { get; set; }
        public string ProductQuality { get; set; }
    }
    #endregion
    #endregion
}
