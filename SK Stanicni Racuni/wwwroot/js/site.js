// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


        $(document).ready(function () {

            $('#myDatalistInput').on('input', function () {
                var data = $('#myDatalistInput').val()

                $.ajax({
                    type: "GET",
                    url: "/RacuniUnutrasnjiSaobracaj/ListaStanica",
                    cache: false,
                    data: { data: data },
                    success: function (data) {
                        var options = '';

                        for (var i = 0; i < data.length; i++) {
                            options += '<option value="' + data[i] + '" />';
                        }
                        $('#myDatalist').html(options)

                    }
                });
            })

        })


        $(window).on('load', function () {

            $.fn.datepicker.dates['rs'] = {
                days: ["Nedelja", "Ponedeljak", "Utorak", "Sreda", "Četvrtak", "Petak", "Subota"],
                daysShort: ["Ned", "Pon", "Uto", "Sre", "Čet", "Pet", "Sub"],
                daysMin: ["Ne", "Po", "Ut", "Sr", "Če", "Pe", "Su"],
                months: ["Januar", "Februar", "Mart", "April", "Maj", "Jun", "Jul", "Avgust", "Septembar", "Oktobar", "Novembar", "Decembar"],
                monthsShort: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Avg", "Sep", "Okt", "Nov", "Dec"],
                today: "Danas",
                clear: "Clear",
                // format: "mm/dd/yyyy",
                //titleFormat: "MM yyyy", /* Leverages same syntax as 'format' */
                weekStart: 1
            };

            $('#DatumOd, #DatumDo').datepicker({
                todayBtn: true,
                todayBtn: 'linked',
                todayHighlight: true,
                format: 'dd.mm.yyyy',
                autoclose: true,
                language: 'rs'
            })
        })


