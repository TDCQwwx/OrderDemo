using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using OrderDemo.Common;
using OrderDemo.Models;
using OrderDemo.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace OrderDemo.ViewModels
{
    public class UserViewModel : NotificationObject
    {
        #region singleton
        private static volatile UserViewModel _instanceUserViewModel;
        private static readonly object obj = new object();
        //private UserViewModel() { }
        public static UserViewModel InstanceUserViewModel
        {
            get
            {
                if (null == _instanceUserViewModel)
                {
                    lock (obj)
                    {
                        if (null == _instanceUserViewModel)
                        {
                            _instanceUserViewModel = new UserViewModel();
                        }
                    }
                }
                return _instanceUserViewModel;
            }
        }
        #endregion

        #region Fields
        private ContentControl _currentUserControl;
        private Visibility _gridHome;
        private Visibility _gridCurrentUserControl;
        private ObservableCollection<OrderStateModel> _orderStateList;
        private DispatcherTimer _timer;//用于取件成功后弹出的界面，延时10秒后返回订单状态信息界面
        private int _second;
        private string _markingInfo;//打标信息
        private string _exteriorInfo;//轮廓外形信息
        private double _sliderValue1;
        private double _sliderValue2;
        private double _sliderValue3;
        private double _sliderValue4;
        private double _sliderValue5;
        private string paraUserName = @"^[\u4e00-\u9fa5a-zA-Z]+$";//用户名,仅支持中文，小写英文字母和大写英文字母.2-8位
        private bool registerUserNameSuccess = false;//匹配正则表达式的标志
        private string paraPhoneNumber = @"^([1]+[3,5,8]+[0-9]+\d{8})$";//手机号码，11位的数字
        private bool registerPhoneNumberSuccess = false;//匹配正则表达式的标志
        private string paraPassword = @"^[A-Za-z0-9]{4,8}$";//密码，大写字母，小写字母和数字，4-8位
        private bool registerPasswordSuccess = false;//匹配正则表达式的标志
        private bool registerSuccess = false;//注册成功的标志
        private string _orderLoginUserName;//下单登录页用户名
        private string _orderLoginPassword;//下单登录页密码
        private string _OrderRegisterUserName;//下单注册页用户名
        private string _orderRegisterPhoneNumber;//下单注册页手机号
        private string _orderRegisterPassword;//下单注册页密码
        private string _takeOrderLoginUserNameorPhoneNumber;//取单登录账号
        private string _takeOrderLoginPassword;//取单登录密码
        private string _registerResultUserName;//注册结果页用户名
        private string _registerResultPhoneNumber;//注册结果页手机号
        private string _registerResultPassword;//注册结果页密码
        private string _orderSubmitResult;//订单提交结果页，用于显示订单提交成功后的具体订单号
        #endregion
      
        #region Attributes
        public ContentControl CurrentUserControl
        {
            get { return _currentUserControl; }
            set
            {
                _currentUserControl = value;
                this.RaisePropertyChanged("CurrentUserControl");
            }
        }
        public ObservableCollection<OrderStateModel> OrderStateList
        {
            get { return _orderStateList; }
            set
            {
                _orderStateList = value;
                this.RaisePropertyChanged("OrderStateList");
            }
        }
        public Visibility GridHome
        {
            get { return _gridHome; }
            set
            {
                _gridHome = value;
                this.RaisePropertyChanged("GridHome");
            }
        }
        public Visibility GridCurrentUserControl
        {
            get { return _gridCurrentUserControl; }
            set
            {
                _gridCurrentUserControl = value;
                this.RaisePropertyChanged("GridCurrentUserControl");
            }
        }
        public int Second
        {
            get { return _second; }
            set
            {
                _second = value;
                this.RaisePropertyChanged("Second");
            }
        }
        public string MarkingInfo
        {
            get { return _markingInfo; }
            set { _markingInfo = value; this.RaisePropertyChanged("MarkingInfo"); }
        }
        public string ExteriorInfo
        {
            get { return _exteriorInfo; }
            set
            {
                _exteriorInfo = value;
                this.RaisePropertyChanged("ExteriorInfo");
            }
        }
        public double SliderValue1
        {
            get { return _sliderValue1; }
            set
            {
                _sliderValue1 = value;
                this.RaisePropertyChanged("SliderValue1");
            }
        }
        public double SliderValue2
        {
            get { return _sliderValue2; }
            set
            {
                _sliderValue2 = value;
                this.RaisePropertyChanged("SliderValue2");
            }
        }
        public double SliderValue3
        {
            get { return _sliderValue3; }
            set
            {
                _sliderValue3 = value;
                this.RaisePropertyChanged("SliderValue3");
            }
        }
        public double SliderValue4
        {
            get { return _sliderValue4; }
            set
            {
                _sliderValue4 = value;
                this.RaisePropertyChanged("SliderValue4");
            }
        }
        public double SliderValue5
        {
            get { return _sliderValue5; }
            set
            {
                _sliderValue5 = value;
                this.RaisePropertyChanged("SliderValue5");
            }
        }
        public string OrderLoginUserName
        {
            get { return _orderLoginUserName; }
            set
            {
                _orderLoginUserName = value;
                this.RaisePropertyChanged("OrderLoginUserName");
            }
        }
        public string OrderLoginPassword
        {
            get { return _orderLoginPassword; }
            set
            {
                _orderLoginPassword = value;
                this.RaisePropertyChanged("OrderLoginPassword");
            }
        }
        public string OrderRegisterUserName
        {
            get { return _OrderRegisterUserName; }
            set
            {
                _OrderRegisterUserName = value;
                this.RaisePropertyChanged("OrderRegisterUserName");
            }
        }
        public string OrderRegisterPhoneNumber
        {
            get { return _orderRegisterPhoneNumber; }
            set
            {
                _orderRegisterPhoneNumber = value;
                this.RaisePropertyChanged("OrderRegisterPhoneNumber");
            }
        }
        public string OrderRegisterPassword
        {
            get { return _orderRegisterPassword; }
            set
            {
                _orderRegisterPassword = value;
                this.RaisePropertyChanged("OrderRegisterPassword");
            }
        }
        public string TakeOrderLoginUserNameorPhoneNumber
        {
            get { return _takeOrderLoginUserNameorPhoneNumber; }
            set
            {
                _takeOrderLoginUserNameorPhoneNumber = value;
                this.RaisePropertyChanged("TakeOrderLoginUserNameorPhoneNumber");
            }
        }
        public string TakeOrderLoginPassword
        {
            get { return _takeOrderLoginPassword; }
            set
            {
                _takeOrderLoginPassword = value;
                this.RaisePropertyChanged("TakeOrderLoginPassword");
            }
        }
        public string RegisterResultUserName
        {
            get { return _registerResultUserName; }
            set
            {
                _registerResultUserName = value;
                this.RaisePropertyChanged("RegisterResultUserName");
            }
        }
        public string RegisterResultPhoneNumber
        {
            get { return _registerResultPhoneNumber; }
            set
            {
                _registerResultPhoneNumber = value;
                this.RaisePropertyChanged("RegisterResultPhoneNumber");
            }
        }
        public string RegisterResultPassword
        {
            get { return _registerResultPassword; }
            set
            {
                _registerResultPassword = value;
                this.RaisePropertyChanged("RegisterResultPassword");
            }
        }
        public string OrderSubmitResult
        {
            get { return _orderSubmitResult; }
            set
            {
                _orderSubmitResult = value;
                this.RaisePropertyChanged("OrderSubmitResult");
            }
        }
        public UCOrderLoginandRegister UCOrderLoginandRegister { get; set; }
        public UCOrderExterior UCOrderExterior { get; set; }
        public UCOrderMarking UCOrderMarking { get; set; }
        public UCOrderSubmitOrder UCOrderSubmitOrder { get; set; }
        public UCOrderSubmitOrderResult UCOrderSubmitOrderResult { get; set; }
        public UCOrderRegister UCOrderRegister { get; set; }
        public UCOrderRegisterResult UCOrderRegisterResult { get; set; }
        public UCTakeOrderLogin UCTakeOrderLogin { get; set; }
        public UCTakeOrderOrderState UCTakeOrderOrderState { get; set; }
        public UCTakeOrderSubmitOrderResult UCTakeOrderSubmitOrderResult { get; set; }

        public DelegateCommand WndHomeOrderCommand { get; set; }//主页——下单命令
        public DelegateCommand WndHomeTakeOrderCommand { get; set; }//主页——取件命令
        public DelegateCommand UCOrderLoginandRegisterLoginCommand { get; set; }//下单登录注册页——登录命令
        public DelegateCommand UCOrderLoginandRegisterRegisterCommand { get; set; }//下单登录注册页——注册命令
        public DelegateCommand UCOrderExteriorSwitchtoUCOrderMarkingCommand { get; set; }//外观轮廓页——切换至打标信息页
        public DelegateCommand UCOrderExteriorBacktoWndHomeCommand { get; set; }//外观轮廓页——返回至主页

        public DelegateCommand UCOrderMarkingSwitchtoUCOrderUCOrderExteriorCommand { get; set; }//打标信息页——切换至外观轮廓页
        public DelegateCommand UCOrderMarkingSwitchtoUCOrderSubmitOrderCommand { get; set; }//打标信息页——切换至产品订单提交页
        public DelegateCommand UCOrderSubmitOrderSwitchtoUCOrderMarkingCommand { get; set; }//产品订单提交页——切换至打标信息页
        public DelegateCommand UCOrderSubmitOrderSubmitConfirmCommand { get; set; }//产品订单提交页——确认提交订单
        public DelegateCommand UCOrderSubmitOrderResultSwitchtoUCOrderSubmitOrderCommand { get; set; }//产品订单提交结果页——切换至产品订单提交页
        public DelegateCommand UCOrderSubmitOrderResultBacktoWndHomeCommand{ get; set; }//产品订单提交结果页——返回至主页
        public DelegateCommand UCOrderRegisterRegisterCommand { get; set; }//注册页——注册命令
        public DelegateCommand UCOrderRegisterSwitchtoUCOrderLoginandRegisterCommand { get; set; }//注册页——切换至注册结果页
        public DelegateCommand UCOrderSubmitOrderResultConfirmCommand { get; set; }//注册结果页——确认命令
        public DelegateCommand UCTakeOrderLoginQueryOrderCommand { get; set; }//取件登录页——查询命令
        public DelegateCommand UCTakeOrderLoginBacktoWndHomeCommand { get; set; }//取件登录页——返回主页
        public DelegateCommand UCTakeOrderOrderStateSwitchtoUCTakeOrderLoginCommand { get; set; }//订单状态页——切换至取件查询登录页
        public DelegateCommand UCTakeOrderOrderStateBacktoWndHomeCommand { get; set; }//订单状态页——返回至主页
        public DelegateCommand<object> UCTakeOrderOrderStateSubmitOrderCommand { get; set; }//订单状态页——提交取件命令
        public DelegateCommand UCTakeOrderOrderStateQueryRefreshCommand { get; set; }//订单状态页——查询订单状态刷新指令
        #endregion

        #region constructor
        public UserViewModel()
        {
            _currentUserControl = new ContentControl();
            _orderStateList = new ObservableCollection<OrderStateModel>();
            GridHomeIsVisible();
            WndHomeOrderCommand = new DelegateCommand(new Action(WndHomeOrderCommandExecute));
            WndHomeTakeOrderCommand = new DelegateCommand(new Action(WndHomeTakeOrderCommandExecute));
            UCOrderLoginandRegisterLoginCommand = new DelegateCommand(new Action(UCOrderLoginandRegisterLoginCommandExecute));
            UCOrderLoginandRegisterRegisterCommand = new DelegateCommand(new Action(UCOrderLoginandRegisterRegisterCommandExecute));
            UCOrderExteriorSwitchtoUCOrderMarkingCommand = new DelegateCommand(new Action(UCOrderExteriorSwitchtoUCOrderMarkingCommandExecute));
            UCOrderExteriorBacktoWndHomeCommand = new DelegateCommand(new Action(this.UCOrderExteriorBacktoWndHomeCommandExecute));
            UCOrderMarkingSwitchtoUCOrderUCOrderExteriorCommand = new DelegateCommand(new Action(UCOrderMarkingSwitchtoUCOrderUCOrderExteriorCommandExecute));
            UCOrderMarkingSwitchtoUCOrderSubmitOrderCommand = new DelegateCommand(new Action(UCOrderMarkingSwitchtoUCOrderSubmitOrderCommandExecute));
            UCOrderSubmitOrderSwitchtoUCOrderMarkingCommand = new DelegateCommand(new Action(UCOrderSubmitOrderSwitchtoUCOrderMarkingCommandExecute));
            UCOrderSubmitOrderSubmitConfirmCommand = new DelegateCommand(new Action(UCOrderSubmitOrderSubmitConfirmCommandExecute));
            UCOrderSubmitOrderResultSwitchtoUCOrderSubmitOrderCommand = new DelegateCommand(new Action(UCOrderSubmitOrderResultSwitchtoUCOrderSubmitOrderCommandExecute));
            UCOrderSubmitOrderResultBacktoWndHomeCommand = new DelegateCommand(new Action(UCOrderSubmitOrderResultBacktoWndHomeCommandExecute));
            UCOrderRegisterRegisterCommand = new DelegateCommand(new Action(this.UCOrderRegisterRegisterCommandExecute));
            UCOrderRegisterSwitchtoUCOrderLoginandRegisterCommand = new DelegateCommand(UCOrderRegisterSwitchtoUCOrderLoginandRegisterCommandExecute);
            UCOrderSubmitOrderResultConfirmCommand = new DelegateCommand(new Action(UCOrderSubmitOrderResultConfirmCommandExecute));
            UCTakeOrderLoginQueryOrderCommand = new DelegateCommand(new Action(UCTakeOrderLoginQueryOrderCommandExecute));
            UCTakeOrderLoginBacktoWndHomeCommand = new DelegateCommand(new Action(UCTakeOrderLoginBacktoWndHomeCommandExecute));
            UCTakeOrderOrderStateSwitchtoUCTakeOrderLoginCommand = new DelegateCommand(new Action(UCTakeOrderOrderStateSwitchtoUCTakeOrderLoginCommandExecute));
            UCTakeOrderOrderStateBacktoWndHomeCommand = new DelegateCommand(new Action(UCTakeOrderOrderStateBacktoWndHomeCommandExecute));
            UCTakeOrderOrderStateSubmitOrderCommand = new DelegateCommand<object>(new Action<object>(UCTakeOrderOrderStateSubmitOrderCommandExecute));
            UCTakeOrderOrderStateQueryRefreshCommand = new DelegateCommand(new Action(UCTakeOrderOrderStateQueryRefreshCommandExecute));
        }

        #endregion

        #region Methods

        #region 首页
        /// <summary>
        /// 下单命令
        /// </summary>
        private void WndHomeOrderCommandExecute()
        {
            GridCurrentUserControlIsVisible();
            InitializeUCOrderLoginandRegister();
            UserControlSwitchFunc(UserControlSwitchEnum.UCOrderLoginandRegister);
        }
        /// <summary>
        /// 取件命令
        /// </summary>
        private void WndHomeTakeOrderCommandExecute()
        {
            GridCurrentUserControlIsVisible();
            InitializeUCTakeOrderLogin();
            UserControlSwitchFunc(UserControlSwitchEnum.UCTakeOrderLogin);
        }
        #endregion

        #region 下单
        /// <summary>
        /// 登录注册页——登录命令
        /// </summary>
        private async void UCOrderLoginandRegisterLoginCommandExecute()
        {
            try
            {
                if (OrderLoginUserName != "" && OrderLoginPassword != "")
                {
                    ResultData resultData = await HTTPHelper.Login(LoginModeEnum.LoginbyUserName, OrderLoginUserName, OrderLoginPassword);
                    if (resultData.Code == ErrorCode.OK)//登录成功
                    {
                        UserControlSwitchFunc(UserControlSwitchEnum.UCOrderExterior);
                    }
                    if (resultData.Code == ErrorCode.WrongParameter)//用户名或密码错误
                    {
                        MessageBox.Show("用户名或密码错误！");
                    }
                }
                else
                {
                    MessageBox.Show("用户名或密码不能为空！");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("访问HTTP服务器时出错！");
            }
        }
        /// <summary>
        /// 登录注册页——注册命令（切换到注册页）
        /// </summary>
        private void UCOrderLoginandRegisterRegisterCommandExecute()
        {
            GridCurrentUserControlIsVisible();
            InitializeUCOrderRegister();
            UserControlSwitchFunc(UserControlSwitchEnum.UCOrderRegister);
        }
        /// <summary>
        /// 注册页——注册命令（切换至注册结果页）
        /// </summary>
        private async void UCOrderRegisterRegisterCommandExecute()
        {
            GridCurrentUserControlIsVisible();

            registerUserNameSuccess = false;//默认匹配正则表达式未成功
            registerPhoneNumberSuccess = false;//默认匹配正则表达式未成功
            registerPasswordSuccess = false;//默认匹配正则表达式未成功
            registerSuccess = false;//默认未注册成功

            InitializeUCOrderRegisterResult();

            //正则表达式判断用户名是否正确
            if (OrderRegisterUserName != "" && OrderRegisterUserName != null)//用户名不为空
            {
                Match matchRegisterUserName = Regex.Match(OrderRegisterUserName, paraUserName); // 匹配正则表达式
                if (matchRegisterUserName.Success && (OrderRegisterUserName.Length >= 2 && OrderRegisterUserName.Length <= 8))
                {
                    registerUserNameSuccess = true;
                }
                else
                {
                    OrderRegisterUserName = "";//清空用户名
                    RegisterResultUserName = "用户名格式不正确，请输入正确的用户名！";
                }
            }
            else//用户名为空
            {
                RegisterResultUserName = "用户名不能为空，请输入用户名！";
            }
            //正则表达式判断手机号是否正确
            if (OrderRegisterPhoneNumber != "" && OrderRegisterPhoneNumber != null)
            {
                Match matchRegisterPhoneNumber = Regex.Match(OrderRegisterPhoneNumber, paraPhoneNumber);   // 匹配正则表达式
                if (matchRegisterPhoneNumber.Success && OrderRegisterPhoneNumber.Length == 11)
                {
                    registerPhoneNumberSuccess = true;
                }
                else
                {
                    OrderRegisterPhoneNumber = "";//清空手机号
                    RegisterResultPhoneNumber = "手机号格式不正确，请输入正确的手机号！";
                }
            }
            else
            {
                RegisterResultPhoneNumber = "手机号不能为空，请输入手机号！";
            }
            //正则表达式判断密码是否正确
            if (OrderRegisterPassword != "" && OrderRegisterPassword != null)
            {
                Match matchRegisterPassword = Regex.Match(OrderRegisterPassword, paraPassword);   // 匹配正则表达式
                if (matchRegisterPassword.Success && (OrderRegisterPassword.Length >= 4 && OrderRegisterPassword.Length <= 8))
                {
                    registerPasswordSuccess = true;
                }
                else
                {
                    OrderRegisterPassword = "";//清空密码
                    RegisterResultPassword = "密码格式不正确，请输入正确的密码！";
                }
            }
            else
            {
                RegisterResultPassword = "密码不能为空，请输入密码！";
            }
            if (registerUserNameSuccess && registerPhoneNumberSuccess && registerPasswordSuccess)
            {
                try
                {
                    ResultData resultData = await HTTPHelper.Register(OrderRegisterUserName, OrderRegisterPhoneNumber, OrderRegisterPassword);
                    if (resultData.Code == ErrorCode.OK)//注册成功
                    {
                        RegisterResultUserName = "用户名：" + OrderRegisterUserName;
                        RegisterResultPhoneNumber = "手机号：" + OrderRegisterPhoneNumber;
                        RegisterResultPassword = "密码：" + OrderRegisterPassword;
                        registerSuccess = true;
                    }
                    if (resultData.Code == ErrorCode.UserNameExisted)//用户名存在
                    {
                        RegisterResultUserName = "用户名已存在，请重新注册！";
                        RegisterResultPhoneNumber = "";
                        RegisterResultPassword = "";
                    }
                    if (resultData.Code == ErrorCode.PhoneNumberExisted)//手机号存在
                    {
                        RegisterResultUserName = "";
                        RegisterResultPhoneNumber = "手机号已存在，请重新注册！";
                        RegisterResultPassword = "";
                    }
                }
                catch (Exception)
                {
                    RegisterResultUserName = "";
                    RegisterResultPhoneNumber = "访问HTTP服务器时出错！";
                    RegisterResultPassword = "";
                }
            }
            UserControlSwitchFunc(UserControlSwitchEnum.UCOrderRegisterResult);
        }
        /// <summary>
        /// 注册页——切换至登录和注册页
        /// </summary>
        private void UCOrderRegisterSwitchtoUCOrderLoginandRegisterCommandExecute()
        {
            GridCurrentUserControlIsVisible();
            InitializeUCOrderLoginandRegister();
            UserControlSwitchFunc(UserControlSwitchEnum.UCOrderLoginandRegister);
        }
        /// <summary>
        /// 注册结果页——确认命令（注册成功返回登录界面；注册失败返回注册界面）
        /// </summary>
        private void UCOrderSubmitOrderResultConfirmCommandExecute()
        {
            GridCurrentUserControlIsVisible();
            if (registerSuccess)
            {
                InitializeUCOrderLoginandRegister();
                UserControlSwitchFunc(UserControlSwitchEnum.UCOrderLoginandRegister);
            }
            else
            {
                InitializeUCOrderRegister();
                UserControlSwitchFunc(UserControlSwitchEnum.UCOrderRegister);
            }
        }
        /// <summary>
        /// 外观轮廓页——返回至主页(涉及从服务器退出)
        /// </summary>
        private async void UCOrderExteriorBacktoWndHomeCommandExecute()
        {
            try
            {
                ResultData resultData = await HTTPHelper.Exit();
                if (resultData.Code == ErrorCode.OK)
                {
                    GridHomeIsVisible();//退出成功后返回主页
                }
                if (resultData.Code == ErrorCode.Exception)
                {
                    MessageBox.Show("异常！");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("访问HTTP服务器时出错！");
            }
        }
        /// <summary>
        /// 外观轮廓页——切换至打标信息页
        /// </summary>
        private void UCOrderExteriorSwitchtoUCOrderMarkingCommandExecute()
        {
            GridCurrentUserControlIsVisible();
            UserControlSwitchFunc(UserControlSwitchEnum.UCOrderMarking);
        }
        /// <summary>
        /// 打标信息页——切换至外观轮廓页
        /// </summary>
        private void UCOrderMarkingSwitchtoUCOrderUCOrderExteriorCommandExecute()
        {
            GridCurrentUserControlIsVisible();
            UserControlSwitchFunc(UserControlSwitchEnum.UCOrderExterior);
        }
        /// <summary>
        /// 打标信息页——切换至订单提交页
        /// </summary>
        private void UCOrderMarkingSwitchtoUCOrderSubmitOrderCommandExecute()
        {
            GridCurrentUserControlIsVisible();
            UserControlSwitchFunc(UserControlSwitchEnum.UCOrderSubmitOrder);

            ExteriorInfo = (29 - SliderValue2 / 8).ToString("0.00") + "," +
                           (29 - SliderValue3 / 8).ToString("0.00") + "," +
                           (29 - SliderValue4 / 8).ToString("0.00");
        }
        /// <summary>
        /// 订单提交页——切换至打标信息页
        /// </summary>
        private void UCOrderSubmitOrderSwitchtoUCOrderMarkingCommandExecute()
        {
            GridCurrentUserControlIsVisible();
            UserControlSwitchFunc(UserControlSwitchEnum.UCOrderMarking);
        }
        /// <summary>
        /// 订单提交页——确认提交订单
        /// </summary>
        private async void UCOrderSubmitOrderSubmitConfirmCommandExecute()
        {
            GridCurrentUserControlIsVisible();
            try
            {
                AddOrderResult addOrderResult = await HTTPHelper.SubmitOrder(MarkingInfo, GetCoordinates());
                if (addOrderResult.Result == "OK")
                {
                    OrderSubmitResult = "您的订单提交成功，订单号为：" + addOrderResult.OrderID;
                    UserControlSwitchFunc(UserControlSwitchEnum.UCOrderSubmitOrderResult);
                }
                if (addOrderResult.Result == "NA")
                {
                    if (addOrderResult.OrderID == "0000")
                    {
                        MessageBox.Show("未登录！");
                    }
                    if (addOrderResult.OrderID == "1111")
                    {
                        MessageBox.Show("wcf服务报错！");
                    }
                    if (addOrderResult.OrderID == "error")
                    {
                        MessageBox.Show("下单异常！");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("访问HTTP服务器时出错！");
            }
        }
        /// <summary>
        /// 产品订单提交结果页——返回至产品订单提交页
        /// </summary>
        private void UCOrderSubmitOrderResultSwitchtoUCOrderSubmitOrderCommandExecute()
        {
            GridCurrentUserControlIsVisible();
            UserControlSwitchFunc(UserControlSwitchEnum.UCOrderSubmitOrder);
        }
        /// <summary>
        /// 产品订单提交结果页——返回至主页（涉及从服务器退出）
        /// </summary>
        private async void UCOrderSubmitOrderResultBacktoWndHomeCommandExecute()
        {
            try
            {
                ResultData resultData = await HTTPHelper.Exit();
                if (resultData.Code == ErrorCode.OK)
                {
                    GridHomeIsVisible();//退出成功后返回主页
                }
                if (resultData.Code == ErrorCode.Exception)
                {
                    MessageBox.Show("异常！");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("访问HTTP服务器时吃错！");
            }
        }
        #endregion

        #region 取单
        /// <summary>
        /// 订单登录页——返回至主页(不涉及从服务器退出)
        /// </summary>
        private void UCTakeOrderLoginBacktoWndHomeCommandExecute()
        {
            GridHomeIsVisible();
        }
        /// <summary>
        /// 订单登录页——查询命令（等价于登录+查询订单）
        /// </summary>
        private async void UCTakeOrderLoginQueryOrderCommandExecute()
        {
            ResultData resultData = null;
            try
            {
                if (TakeOrderLoginUserNameorPhoneNumber.Length == 11)
                {
                    resultData = await HTTPHelper.Login(LoginModeEnum.LoginbyPhoneNumber, TakeOrderLoginUserNameorPhoneNumber, TakeOrderLoginPassword);
                }
                else
                {
                    resultData = await HTTPHelper.Login(LoginModeEnum.LoginbyUserName, TakeOrderLoginUserNameorPhoneNumber, TakeOrderLoginPassword);
                }
                if (resultData.Code == ErrorCode.OK)//取单登录成功后，进行订单查询
                {
                    OrderStateListDto orderStateListDto = await HTTPHelper.QueryOrder();

                    App.Current.Dispatcher.Invoke(() => { OrderStateList.Clear(); });//更新新的数据之前先清空列表

                    foreach (var item in orderStateListDto.States)
                    {
                        var list = new OrderStateModel
                        {
                            OrderID = item.OrderID,
                            OrderStartTime = item.OrderStartTime,
                            CurrentState = item.CurrentState,
                            ProcessingName = item.ProcessingName,
                            TakeOrderIsOK = item.CurrentState == "Finished",
                        };
                        App.Current.Dispatcher.Invoke(() => { OrderStateList.Add(list); });
                    }
                    UserControlSwitchFunc(UserControlSwitchEnum.UCTakeOrderOrderState);
                }
                if (resultData.Code == ErrorCode.WrongParameter)//用户名或密码错误
                {
                    MessageBox.Show("用户名或密码错误！");
                }
                if (resultData.Code == ErrorCode.Exception)//异常
                {
                    MessageBox.Show("异常！");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("访问HTTP服务器时出错！");
            }
        }
        /// <summary>
        /// 订单状态页——提交取件命令
        /// </summary>
        /// <param name="obj">订单号</param>
        private async void UCTakeOrderOrderStateSubmitOrderCommandExecute(object obj)
        {
            GridCurrentUserControlIsVisible();
            try
            {
                ResultData resultData = await HTTPHelper.TakeOrder(obj.ToString());
                if (resultData.Result == "OK")//取单递交成功
                {
                    if (resultData.Code == ErrorCode.OK)
                    {
                        UserControlSwitchFunc(UserControlSwitchEnum.UCTakeOrderSubmitOrderResult);
                        TimerStart(10);
                    }
                }
                if (resultData.Result == "NA")//取单递交失败
                {
                    if (resultData.Code == ErrorCode.UserNotLogined)
                    {
                        MessageBox.Show("用户未登录！");
                    }
                    if (resultData.Code == ErrorCode.Exception)
                    {
                        MessageBox.Show("异常！");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("访问HTTP服务器时出错！");
            }
        }
        /// <summary>
        /// 订单状态页——查询订单状态刷新指令
        /// </summary>
        private async void UCTakeOrderOrderStateQueryRefreshCommandExecute()
        {
            try
            {
                OrderStateListDto orderStateListDtos = await HTTPHelper.QueryOrder();

                App.Current.Dispatcher.Invoke(() => { OrderStateList.Clear(); });//更新新的数据之前先清空列表
                foreach (var item in orderStateListDtos.States)
                {
                    var list = new OrderStateModel
                    {
                        OrderID = item.OrderID,
                        OrderStartTime = item.OrderStartTime,
                        CurrentState = item.CurrentState,
                        ProcessingName = item.ProcessingName,
                        TakeOrderIsOK = item.CurrentState == "Finished",
                    };
                    App.Current.Dispatcher.Invoke(() => { OrderStateList.Add(list); });
                }
            }
            catch (Exception)
            {
                MessageBox.Show("访问HTTP服务器时出错！");
            }
        }
        /// <summary>
        /// 订单状态页——切换至取件登录页(涉及从服务器退出)
        /// </summary>
        private async void UCTakeOrderOrderStateSwitchtoUCTakeOrderLoginCommandExecute()
        {
            GridCurrentUserControlIsVisible();
            try
            {
                ResultData resultData = await HTTPHelper.Exit();
                if (resultData.Code == ErrorCode.OK)//退出成功后返回下单登录页
                {
                    InitializeUCTakeOrderLogin();
                    UserControlSwitchFunc(UserControlSwitchEnum.UCTakeOrderLogin);
                }
                if (resultData.Code == ErrorCode.Exception)
                {
                    MessageBox.Show("异常！");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("访问HTTP服务器时出错！");
            }
        }
        /// <summary>
        /// 订单状态页——返回至主页（涉及从服务器退出）
        /// </summary>
        private async void UCTakeOrderOrderStateBacktoWndHomeCommandExecute()
        {
            try
            {
                ResultData resultData = await HTTPHelper.Exit();
                if (resultData.Code == ErrorCode.OK)
                {
                    GridHomeIsVisible();//退出成功后返回主页
                }
                if (resultData.Code == ErrorCode.Exception)
                {
                    MessageBox.Show("异常！");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("访问HTTP服务器时出错！");
            }
        }
        #endregion

        #region 无数据交互
        /// <summary>
        /// UserControl切换函数
        /// </summary>
        /// <param name="userControlSwitchEnum"></param>
        public void UserControlSwitchFunc(UserControlSwitchEnum userControlSwitchEnum)
        {
            switch (userControlSwitchEnum)
            {
                case UserControlSwitchEnum.UCOrderLoginandRegister:
                    if (UCOrderLoginandRegister == null) UCOrderLoginandRegister = new UCOrderLoginandRegister();
                    CurrentUserControl.Content = UCOrderLoginandRegister;
                    break;
                case UserControlSwitchEnum.UCOrderExterior:
                    if (UCOrderExterior == null) UCOrderExterior = new UCOrderExterior();
                    CurrentUserControl.Content = UCOrderExterior;
                    break;
                case UserControlSwitchEnum.UCOrderMarking:
                    if (UCOrderMarking == null) UCOrderMarking = new UCOrderMarking();
                    CurrentUserControl.Content = UCOrderMarking;
                    break;
                case UserControlSwitchEnum.UCOrderSubmitOrder:
                    if (UCOrderSubmitOrder == null) UCOrderSubmitOrder = new UCOrderSubmitOrder();
                    CurrentUserControl.Content = UCOrderSubmitOrder;
                    break;
                case UserControlSwitchEnum.UCOrderSubmitOrderResult:
                    if (UCOrderSubmitOrderResult == null) UCOrderSubmitOrderResult = new UCOrderSubmitOrderResult();
                    CurrentUserControl.Content = UCOrderSubmitOrderResult;
                    break;
                case UserControlSwitchEnum.UCOrderRegister:
                    if (UCOrderRegister == null) UCOrderRegister = new UCOrderRegister();
                    CurrentUserControl.Content = UCOrderRegister;
                    break;
                case UserControlSwitchEnum.UCOrderRegisterResult:
                    if (UCOrderRegisterResult == null) UCOrderRegisterResult = new UCOrderRegisterResult();
                    CurrentUserControl.Content = UCOrderRegisterResult;
                    break;
                case UserControlSwitchEnum.UCTakeOrderLogin:
                    if (UCTakeOrderLogin == null) UCTakeOrderLogin = new UCTakeOrderLogin();
                    CurrentUserControl.Content = UCTakeOrderLogin;
                    break;
                case UserControlSwitchEnum.UCTakeOrderOrderState:
                    if (UCTakeOrderOrderState == null) UCTakeOrderOrderState = new UCTakeOrderOrderState();
                    CurrentUserControl.Content = UCTakeOrderOrderState;
                    break;
                case UserControlSwitchEnum.UCTakeOrderSubmitOrderResult:
                    if (UCTakeOrderSubmitOrderResult == null) UCTakeOrderSubmitOrderResult = new UCTakeOrderSubmitOrderResult();
                    CurrentUserControl.Content = UCTakeOrderSubmitOrderResult;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 主页显示
        /// </summary>
        public void GridHomeIsVisible()
        {
            GridCurrentUserControl = Visibility.Hidden;
            GridHome = Visibility.Visible;
        }
        /// <summary>
        /// 内容空间显示
        /// </summary>
        public void GridCurrentUserControlIsVisible()
        {
            GridHome = Visibility.Hidden;
            GridCurrentUserControl = Visibility.Visible;
        }
        /// <summary>
        /// 获取所需上传的五个点
        /// </summary>
        /// <returns></returns>
        public List<Coordinate> GetCoordinates()
        {
            var coordinate1 = new Coordinate
            {
                X = double.Parse((29 - SliderValue1 / 8).ToString("0.00")),
                Z = -20
            };
            var coordinate2 = new Coordinate
            {
                X = double.Parse((29 - SliderValue2 / 8).ToString("0.00")),
                Z = -28
            };
            var coordinate3 = new Coordinate
            {
                X = double.Parse((29 - SliderValue3 / 8).ToString("0.00")),
                Z = -36
            };
            var coordinate4 = new Coordinate
            {
                X = double.Parse((29 - SliderValue4 / 8).ToString("0.00")),
                Z = -44
            };
            var coordinate5 = new Coordinate
            {
                X = double.Parse((29 - SliderValue5 / 8).ToString("0.00")),
                Z = -52
            };
            List<Coordinate> coordinates = new List<Coordinate>();
            coordinates.Add(coordinate1);
            coordinates.Add(coordinate2);
            coordinates.Add(coordinate3);
            coordinates.Add(coordinate4);
            coordinates.Add(coordinate5);
            return coordinates;
        }
        /// <summary>
        /// 定时器开始工作
        /// </summary>
        /// <param name="second">定时时长</param>
        public void TimerStart(int second)
        {
            Second = second;
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 1);   //间隔1秒
            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Start();

        }
        /// <summary>
        /// 定时器定时执行的函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            Second--;
            if (Second == 0)
            {
                UserControlSwitchFunc(UserControlSwitchEnum.UCTakeOrderOrderState);
                _timer.Stop();
            }
        }
        public void InitializeUCOrderRegister()
        {
            OrderRegisterUserName = "";
            OrderRegisterPhoneNumber = "";
            OrderRegisterPassword = "";
        }
        public void InitializeUCOrderRegisterResult()
        {
            RegisterResultUserName = "";
            RegisterResultPhoneNumber = "";
            RegisterResultPassword = "";
        }
        public void InitializeUCOrderLoginandRegister()
        {
            OrderLoginUserName = "";
            OrderLoginPassword = "";
        }
        public void InitializeUCTakeOrderLogin()
        {
            TakeOrderLoginUserNameorPhoneNumber = "";
            TakeOrderLoginPassword = "";
        }
        #endregion

        #endregion
    }
}
