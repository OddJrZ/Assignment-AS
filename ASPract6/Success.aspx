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
            User ID:
            <asp:Label ID="lbl_userID" runat="server" Text="test1"></asp:Label>
            <br />
            <br />
            NRIC:
            <asp:Label ID="lbl_nric" runat="server" Text="test2"></asp:Label>
            <br />
            <br />
            <asp:Button ID="btn_Logout" runat="server" Text="Logout" OnClick="LogoutMe" Visible="false"/>
        </div>
            </fieldset>
    </form>
</body>
</html>
