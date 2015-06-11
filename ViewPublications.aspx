<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewPublications.aspx.cs" Inherits="LocalTravelInfo.ViewPublications" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="content/css/bootstrap.css" rel="stylesheet" type="text/css" />

</head>
<body style="padding-top: 60px">
    <div class="navbar navbar-inverse navbar-fixed-top">
        <div class="container">
            <div class="navbar-header">
                <a class="navbar-brand" href="#">London Travel Info</a>
            </div>
            <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                <ul class="nav navbar-nav">
                    <li class="active"><a href="#">Découvrez Londres </a></li>
                    <li><a href="Publish.aspx">New Publication</a></li>
                </ul>
            </div>
        </div>

    </div>


    <div class="container">
        <p class="lead">Découvrez Londres à l'aide des habitants locaux!</p>
        <form id="form1" runat="server" role="form">
            <asp:DataList
                ID="PublicationsList"
                GridLines="None"
                RepeatLayout="Table"
                runat="server">

                <HeaderTemplate>
                    
                </HeaderTemplate>

                <ItemTemplate>
                    <div><a style="font-size:12pt;font-weight:bold" href="DownloadPublication.ashx?id=<%# Eval("Id") %>"><%# Eval("Title") %></a></div>
                    <div><%# Eval("Description") %></div>
                    <br />
                </ItemTemplate>

                <FooterTemplate>
                    
                </FooterTemplate>

            </asp:DataList>
        </form>
    </div>


    <script src="scripts/jquery-2.1.3.min.js"></script>
    <script src="scripts/bootstrap.min.js"></script>


</body>
</html>
