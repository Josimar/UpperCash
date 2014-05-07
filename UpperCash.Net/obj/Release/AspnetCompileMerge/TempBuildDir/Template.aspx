<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Template.aspx.cs" Inherits="UpperCash.Net.Template" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <title>Title</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" type="text/css" href="Content/css/jquery.mobile-1.4.2.css" />
    <script type="text/javascript" src="Content/js/jquery-2.1.0.js"></script>
    <script type="text/javascript" src="Content/js/jquery.mobile-1.4.2.js"></script>
</head>
<body>
    <div data-role="page" data-addback-btn="true" data-back-btntext="Voltar" data-theme="a">
        <div data-role="header">
            <h1>Page Header</h1>
        </div>

        <div data-role="content">
            <p>Hello jQuery Mobile!</p>
            <ul data-role="listview" datafilter="true" data-inset="true" data-count-theme="e">
                <li data-role="list-divider">Comments</p></li>
                <li>
                    <img src="../images/111-user.png" class="ui-li-icon">
                    <p>Thanks for the review. I'll check it out this weekend.</p>
                    <span class="ui-li-count">1 day ago</span>
                </li>
            </ul>
            <div data-role="collapsible-set" data-theme="a" data-content-theme="b">
                <div data-role="collapsible" data-collapsed="true">
                    <h3>Wireless</h3>
                    <ul data-role="listview" data-inset="true">
                        <li><a href="#">&#xe117; Notifications</a></li>
                        <li><a href="#">&#xe01d; Location Services</a></li>
                    </ul>
                </div>
                <div data-role="collapsible">
                    <h3>Applications</h3>
                    <ul data-role="listview" data-inset="true">
                        <li><a href="#">&#xe001; Faceoff</a></li>
                        <li><a href="#">&#xe428; LinkedOut</a></li>
                        <li><a href="#">&#xe03d; Netflicks</a></li>
                    </ul>
                </div>
            </div>
            <a href="" data-role="button" datacorners="false">Disagree</a>
        </div>

        <div data-role="footer" data-position="fixed">
            <div data-role="navbar">
                <ul>
                    <li><a href="#" data-icon="arrow-l"></a></li>
                    <li><a href="#" data-icon="back"></a></li>
                    <li><a href="#" data-icon="star"></a></li>
                    <li><a href="#" data-icon="plus"></a></li>
                    <li><a href="#" data-icon="arrow-r"></a></li>
                </ul>
            </div>
        </div>
    </div>
</body>
</html>
