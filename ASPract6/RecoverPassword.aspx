<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecoverPassword.aspx.cs" Inherits="ASPract6.RecoverPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Account Recovery</title>

            <script type="text/javascript">
        function validate() {
            var str = document.getElementById('<%=tb_pwd.ClientID %>').value;

            if (str.length < 8) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password Length Must be at Least 8 Characters";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("too_short");
            }

            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 number";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_number");
            }

            else if (str.search(/[a-z]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 lowercase";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_lowercase");
            }

            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 uppercase";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_uppercase");
            }

            else if (str.search(/[^\w]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 special character";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_special");
            }

            document.getElementById("lbl_pwdchecker").innerHTML = "Excellent!";
            document.getElementById("lbl_pwdchecker").style.color = "Blue";
            };

        function validateCfmPass() {
            var str = document.getElementById('<%=tb_pwd.ClientID %>').value;
            var str2 = document.getElementById('<%=tb_cfmpwd.ClientID %>').value;

            if (str == str2) {
                document.getElementById("lbl_cfmpwdchecker").innerHTML = "Excellent!";
                document.getElementById("lbl_cfmpwdchecker").style.color = "Blue";
                return true;
            }
            else {
                document.getElementById("lbl_cfmpwdchecker").innerHTML = "Password does not match!";
                document.getElementById("lbl_cfmpwdchecker").style.color = "Red";
                return false;
            }
        }
            
            </script>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style2 {
            width: 148px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <fieldset>
            <legend>Account Recovery</legend>
        <div>
            <br />
            <table class="auto-style1">
                <tr>
                    <td class="auto-style2">Email</td>
                    <td>
                        <asp:TextBox ID="tb_email" runat="server" Width="246px" onkeyup="javascript:validateEmail()" TextMode="Email" ReadOnly="True"></asp:TextBox>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        <asp:Label ID="lbl_newPass" runat="server" Text="New Password"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tb_pwd" runat="server" Width="246px" onkeyup="javascript:validate()" TextMode="Password"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorPassword" runat="server"
                ErrorMessage="Password is required" ForeColor="Red" ControlToValidate="tb_pwd" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:Label ID="lbl_pwdchecker" runat="server"></asp:Label>
                        </td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        <asp:Label ID="lbl_cfmpwd" runat="server" Text="Confirm Password"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tb_cfmpwd" runat="server" Width="246px" onkeyup="javascript:validateCfmPass()" TextMode="Password"></asp:TextBox>
                        <asp:Label ID="lbl_cfmpwdchecker" runat="server"></asp:Label>
                        </td>
                </tr>
                <tr>
                    <td class="auto-style2">&nbsp;</td>
                    <td>
                        <asp:Button ID="btn_Submit" runat="server" Height="31px" OnClick="btn_Submit_Click" Text="Submit" Width="254px" />
                    </td>
                </tr>
            </table>
            <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label>
            <br />
        </div>
            </fieldset>
    </form>
</body>
</html>
