<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Success.aspx.cs" Inherits="ASPract6.Success" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <fieldset>
            <legend>User Profile</legend>
        <div>
            <br />
            <br />
            Email:
            <asp:Label ID="lbl_userID" runat="server" Text="test1"></asp:Label>
            <br />
            <br />
            Name:
            <asp:Label ID="lbl_name" runat="server" Text="test2"></asp:Label>
            <br />
            <br />
            Minimum Password Age: <asp:Label ID="lbl_MinAge" runat="server"></asp:Label>
            <br />
            <asp:Button ID="btn_CheckMin" runat="server" OnClick="btn_CheckMin_Click" Text="Check" Width="147px" />
            <br />
            <br />
            Maximum Password Age:
            <asp:Label ID="lbl_MaxAge" runat="server"></asp:Label>
            <br />
            <asp:Button ID="btn_CheckMax" runat="server" OnClick="btn_CheckMax_Click" Text="Check" Width="150px" />
            <br />
            <br />
            <br />
            <asp:Button ID="btn_Logout" runat="server" Text="Logout" OnClick="LogoutMe" Visible="false" Width="148px"/>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btn_changePass" runat="server" OnClick="btn_changePass_Click" Text="Change Password" Visible="False" />
        </div>
            </fieldset>
    </form>
</body>
</html>
