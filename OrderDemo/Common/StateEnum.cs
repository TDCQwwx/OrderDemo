using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderDemo.Common
{
    public enum UserControlSwitchEnum
    {
        UCOrderLoginandRegister,
        UCOrderExterior,
        UCOrderMarking,
        UCOrderSubmitOrder,
        UCOrderSubmitOrderResult,
        UCOrderRegister,
        UCOrderRegisterResult,
        UCTakeOrderLogin,
        UCTakeOrderOrderState,
        UCTakeOrderSubmitOrderResult
    }
    public enum LoginModeEnum
    {
        LoginbyUserName,
        LoginbyPhoneNumber
    }

    public enum RegisterValidationEnum
    {
        None,
        UserNameIsOk,//用户名合法
        PhoneNumberIsOK,//手机号合法
        PasswordIsOK,//密码合法
        UserNameIsWrong,//用户名格式不正确
        PhoneNumberIsWrong,//手机号格式不正确
        PasswordIsWrong,//密码格式不正确
    }
}
