<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="ASPract6.Registration" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registration</title>
    <script src="https://www.google.com/recaptcha/api.js?render=6Lcsx0IaAAAAAJNoOIkh1UVR3nEMUZl5HzZSQ6Is"></script>

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
            width: 175px;
        }
        .auto-style3 {
            width: 175px;
            height: 23px;
        }
        .auto-style4 {
            height: 23px;
        }
        .auto-style5 {
            width: 175px;
            height: 26px;
        }
        .auto-style6 {
            height: 26px;
        }
        .auto-style7 {
            width: 175px;
            height: 30px;
        }
        .auto-style8 {
            height: 30px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <fieldset>
                <legend>Account Registration</legend>
            
            <br />
            <table class="auto-style1">
                <tr>
                    <td class="auto-style2">First Name</td>
                    <td>
                        <asp:TextBox ID="tb_firstName" runat="server" Width="246px"></asp:TextBox>
                                   <asp:RequiredFieldValidator ID="RequiredFieldValidatorFName" runat="server"
                ErrorMessage="First Name is required" ForeColor="Red" ControlToValidate="tb_firstName" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">Last Name</td>
                    <td>
                        <asp:TextBox ID="tb_lastName" runat="server" Width="246px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorLName" runat="server"
                ErrorMessage="Last Name is required" ForeColor="Red" ControlToValidate="tb_lastName" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style7">Email Address</td>
                    <td class="auto-style8">
                        <asp:TextBox ID="tb_email" runat="server" Width="246px" onkeyup="javascript:validateEmail()" TextMode="Email"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorEmail" runat="server"
                ErrorMessage="Email is required" ForeColor="Red" ControlToValidate="tb_email" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style5">Password</td>
                    <td class="auto-style6">
                        <asp:TextBox ID="tb_pwd" runat="server" Width="246px" onkeyup="javascript:validate()" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorPassword" runat="server"
                ErrorMessage="Password is required" ForeColor="Red" ControlToValidate="tb_pwd" Display="Dynamic"></asp:RequiredFieldValidator>
                    &nbsp;<asp:Label ID="lbl_pwdchecker" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Confirm Password</td>
                    <td class="auto-style4">
                        <asp:TextBox ID="tb_cfmpwd" runat="server" Width="246px" onkeyup="javascript:validateCfmPass()" TextMode="Password"></asp:TextBox>
                        <asp:Label ID="lbl_cfmpwdchecker" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">Date of Birth</td>
                    <td>
                        <asp:TextBox ID="tb_dob" runat="server" Width="246px" TextMode="Date"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorDob" runat="server"
                ErrorMessage="Date of Birth is required" ForeColor="Red" ControlToValidate="tb_dob" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">Card Number</td>
                    <td>
                        <asp:TextBox ID="tb_cardNum" runat="server" Width="246px" TextMode="Number"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorCardNum" runat="server"
                ErrorMessage="Card Number is required" ForeColor="Red" ControlToValidate="tb_cardNum" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">Card CVV</td>
                    <td>
                        <asp:TextBox ID="tb_cardCVV" runat="server" Width="246px" TextMode="Number"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorCardCVV" runat="server"
                ErrorMessage="Card CVV is required" ForeColor="Red" ControlToValidate="tb_cardCVV" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">&nbsp;</td>
                    <td>
                        <asp:Button ID="btn_Submit" runat="server" Height="31px" OnClick="btn_Submit_Click" Text="Submit" Width="254px" />
                    </td>
                </tr>
            </table>
                <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
            <br />
            <asp:Label ID="lbl_error1" runat="server" ForeColor="Red"></asp:Label>
                </fieldset>
        </div>
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
