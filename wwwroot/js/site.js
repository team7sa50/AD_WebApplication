// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('#dt-bordered').dataTable({

        columnDefs:
            [{
                orderable: false,
                className: 'select-checkbox',
                targets: 0
            }],
        select:
        {
            style: 'multi',
            selector: 'td:first-child'
        }
    });
});