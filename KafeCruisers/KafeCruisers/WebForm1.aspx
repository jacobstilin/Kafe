<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="KafeCruisers.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-3.3.1.js"></script>
    <script src="Scripts/jquery.timepicker.js"></script>
    <link href="Scripts/jquery.timepicker.css" rel="stylesheet" />
    <script>
        $(function () {

        $('#disableTimeRangesExample').timepicker({
            'disableTimeRanges': [
                ['1am', '2am'],
                ['3am', '4:01am']
            ]
        })
        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <input type="text" id="disableTimeRangesExample" />
        </div>
    </form>
</body>
</html>
