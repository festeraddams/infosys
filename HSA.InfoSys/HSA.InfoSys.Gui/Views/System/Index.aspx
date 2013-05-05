﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    My Systems
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">
        <div class="contentbox" style="width: 100%;">
            <div class="contentbox-header">
                <i class="icon-align-justify"></i>&nbsp;<b>My Systems</b>
            </div>
            <div class="contentbox-content">
                <table class="table table-hover">
                    <thead>
                        <tr>
                            <th style="width: 35px;">
                                <i class="icon-search"></i>
                            </th>
                            <th style="width: 600px;">
                                System
                            </th>
                            <th>
                                Edit
                            </th>
                            <th>
                                Delete
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>
                                <input type="checkbox" value="1" />
                            </td>
                            <td>
                                Webserver lnx07
                            </td>
                            <td>
                                <a class="btn btn-small" href="/System/Components"><i class="icon-cog"></i></a>
                            </td>
                            <td>
                                <a class="btn btn-small" href="#deleteModal" data-toggle="modal"><i class="icon-trash"></i></a>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input type="checkbox" value="2" />
                            </td>
                            <td>
                                Workstation1
                            </td>
                            <td>
                                <a class="btn btn-small" href="/System/Components"><i class="icon-cog"></i></a>
                            </td>
                            <td>
                                <a class="btn btn-small" href="#deleteModal" data-toggle="modal"><i class="icon-trash"></i></a>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input type="checkbox" value="3" />
                            </td>
                            <td>
                                Backup-Server 002
                            </td>
                            <td>
                                <a class="btn btn-small" href="/System/Components"><i class="icon-cog"></i></a>
                            </td>
                            <td>
                                <a class="btn btn-small" href="#deleteModal" data-toggle="modal"><i class="icon-trash"></i></a>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div id="buttons" style="float: right;">
                <a class="btn btn-success" style="margin-bottom: 10px; margin-right: 10px;" href="#createModal" data-toggle="modal">
                    <i class="icon-plus icon-white"></i>&nbsp;&nbsp;<b>Create new System</b></a>
                <button type="submit" class="btn btn-primary" style="margin-bottom: 10px; margin-right: 10px;">
                    <i class="icon-search icon-white"></i>&nbsp;&nbsp;<b>Search</b></button>
            </div>
        </div>
    </div>

    <!-- createModal -->
    <div id="createModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                ×</button>
            <h3 id="myModalLabel">
                Create new system</h3>
        </div>
        <div class="modal-body">
            <p>Please enter a name for your new system:</p>
            <input type="text" style="width: 440px;" placeholder="i.e. Webserver, Working-Station, Backup-Server">
        </div>
        <div class="modal-footer">
            <button class="btn" data-dismiss="modal" aria-hidden="true">
                Close</button>
            <a class="btn btn-success" href="#">
                <i class="icon-plus icon-white"></i>&nbsp;&nbsp;<b>Create</b></a>
        </div>
    </div>

    <!-- deleteModal -->
    <div id="deleteModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                ×</button>
            <h3 id="H1">
                Delete System</h3>
        </div>
        <div class="modal-body">
            <p>Are you sure you want to delete the following system?</p>
            <span class="label">Webserver</span>
        </div>
        <div class="modal-footer">
            <button class="btn" data-dismiss="modal" aria-hidden="true">
                Close</button>
            <a class="btn btn-danger" href="#">
                <i class="icon-trash icon-white"></i>&nbsp;&nbsp;<b>Delete</b></a>
        </div>
    </div>


    <script>
        $(document).ready(function () {

            // addButton click event
            $('#addButton').click(function () {
                addInputBox();
            });

            // clearAllButton click event
            $('#clearAllButton').click(function () {
                // delete all added InputBoxes
                $('#addedBoxes').empty();
                // set value of mainInputBox to empty
                $('#mainInputBox').val('');
            });

        });

        /**
        * adds a new component-InputBox
        */
        function addInputBox() {

            // get Value of mainInputBox
            var value = $('#mainInputBox').val();

            // clear Value of mainInputBox
            $('#mainInputBox').val('');

            // append the new InputBox
            var newInputBox = '<input class="input-components" type="text" placeholder="component" value="' + value + '" name="components[]">';
            var newButton = '<button type="button" class="btn btn-danger" onclick="removeInputBox($(this))"><i class="icon-minus-sign"></i></button>';
            $('#addedBoxes').append(
                    '<div class="input-append">'
                    + newInputBox
                    + newButton
                    + '</div>'
                );
        }

        /**
        * deletes a component-InputBox
        */
        function removeInputBox(button) {

            // remove the parent element of the button (in this case the div)
            button.parent().remove();

            //$('#addedBoxes').find(':last-child').not(':only-child').remove();
        }

    </script>

</asp:Content>