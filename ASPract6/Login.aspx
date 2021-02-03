<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ASPract6.Login" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login Form</title>
    <script src="https://www.google.com/recaptcha/api.js?render=6Lcsx0IaAAAAAJNoOIkh1UVR3nEMUZl5HzZSQ6Is"></script>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
            width: 165px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <fieldset>
            <legend>Login</legend>
        <div>
            <br />
            <br />
            <table class="auto-style1">
                <tr>
                    <td class="auto-style2">User ID/Email</td>
                    <td>
                        <asp:TextBox ID="tb_email" runat="server" Width="221px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">Password</td>
                    <td>
                        <asp:TextBox ID="tb_pwd" runat="server" Width="221px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">&nbsp;</td>
                    <td>
                        <asp:Button ID="btn_Submit" runat="server" Height="35px" OnClick="btn_Submit_Click" Text="Submit" Width="236px" />
                    </td>
                </tr>
            </table>
            <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
            <br />
            <asp:Label ID="lbl_errorMsg" runat="server" ForeColor="Red"></asp:Label>
            <br />
            <asp:Label ID="lbl_errorXSS" runat="server"></asp:Label>
            <br />
            <asp:Label ID="lbl_gScore" runat="server"></asp:Label>
        </div>
            </fieldset>
    </form>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6Lcsx0IaAAAAAJNoOIkh1UVR3nEMUZl5HzZSQ6Is', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
</body>
</html>
