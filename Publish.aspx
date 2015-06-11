<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Publish.aspx.cs" Inherits="LocalTravelInfo.Publish" Async="true" AsyncTimeout="3600" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="content/css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="content/css/custom.css" rel="stylesheet" type="text/css" />
     <style>.btn-file {
  position: relative;
  overflow: hidden;
}
.btn-file input[type=file] {
  position: absolute;
  top: 0;
  right: 0;
  min-width: 100%;
  min-height: 100%;
  font-size: 100px;
  text-align: right;
  filter: alpha(opacity=0);
  opacity: 0;
  background: red;
  cursor: inherit;
  display: block;
}
input[readonly] {
  background-color: white !important;
  cursor: text !important;
}</style>
    <script src="scripts/jquery-2.1.3.min.js"></script>
    <script src="scripts/bootstrap.min.js"></script>
     <script>
         $(document).on('change', '.btn-file :file', function () {
             var input = $(this), numFiles = input.get(0).files ? input.get(0).files.length : 1, label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
             input.trigger('fileselect', [
                 numFiles,
                 label
             ]);
         });
         $(document).ready(function () {
             $('.btn-file :file').on('fileselect', function (event, numFiles, label) {
                 var input = $(this).parents('.input-group').find(':text'), log = numFiles > 1 ? numFiles + ' files selected' : label;
                 if (input.length) {
                     input.val(log);
                 } else {
                     if (log)
                         alert(log);
                 }
             });
         });
  </script>
</head>
<body style="padding-top: 60px">
    <div class="navbar navbar-inverse navbar-fixed-top">
        <div class="container">
            <div class="navbar-header">
                <a class="navbar-brand" href="Default.aspx">London Travel Info</a>
            </div>
             <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                <ul class="nav navbar-nav">
                    <li ><a href="Default.aspx">Découvrez Londres </a></li>
                    <li class="active"><a href="#">New Publication</a></li>
                </ul>
          </div>
        </div>

    </div>


    <div class="container">
        <form id="form1" runat="server" role="form">
            <p class="lead">Publish travel information about your city, London.</p>
            <div class="form-group">
                <asp:Label ID="TitleLabel" runat="server" Text="Title:"></asp:Label>
                <asp:TextBox ID="TitleTextBox" runat="server"  CssClass="form-control"></asp:TextBox>
            </div>
           
            <div class="input-group">
                <span class="input-group-btn">
                    <span class="btn btn-primary btn-file">
                        Browse&hellip; <asp:FileUpload ID="FileUpload" runat="server"  />
                    </span>
                </span>
                <input type="text" class="form-control" readonly>
            </div>

            <div class="form-group">
                <asp:Label ID="DescriptionLabel" runat="server" Text="Description:"></asp:Label>
                <asp:TextBox ID="DescriptionTextBox" runat="server" Rows="6" Height="94px"  CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
            </div>
            <asp:Button ID="PublishButton" runat="server" Text="Publish" CssClass="btn btn-primary" OnClick="PublishButton_Click" />
        </form>
    </div>


    

   
</body>
</html>
