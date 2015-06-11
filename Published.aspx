<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Published.aspx.cs" Inherits="LocalTravelInfo.Published" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="content/css/bootstrap.css" rel="stylesheet" type="text/css" />
   
</head>
<body style="padding-top: 60px">
    <div class="navbar navbar-inverse navbar-fixed-top">
        <div class="container">
            <a class="navbar-brand" href="ViewPublications.aspx">London Travel Info</a>
        </div>
    </div>


    <div class="container">
        <form id="form1" runat="server" role="form">
            <p class="lead"><asp:Label ID="HeaderLabel" runat="server" ></asp:Label></p>
            <div class="form-group">
                <asp:Label ID="MethodLabel" runat="server" Text="Method:"></asp:Label>
                <asp:TextBox ID="MethodTextBox" runat="server"  CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>

            <div class="form-group">
                <asp:Label ID="TrustScoreLabel" runat="server" Text="TrustScore:"></asp:Label>
                <asp:TextBox ID="TrustScoreTextBox" runat="server"  CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>

            <div class="form-group">
                <asp:Label ID="PriceLabel" runat="server" Text="Price:"></asp:Label>
                <asp:TextBox ID="PriceTextBox" runat="server"  CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>
           
            <input type="button" onclick="window.location = '/ViewPublications.aspx'" value="Back" class="btn btn-primary"/>
            
        </form>
    </div>


    <script src="scripts/jquery-2.1.3.min.js" />
    <script src="scripts/bootstrap.min.js" />

    
</body>
</html>
