// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


        $(document).ready(function () {


            $('#myDatalistInput').on('input', function () {
                var data = $('#myDatalistInput').val()

                var getUrl = window.location;
                var baseUrl = getUrl.protocol + "//" + getUrl.host + "/" + getUrl.pathname.split('/')[1];

                //console.log("baseUrl " + baseUrl)
                //ovo radi na hostingu, na mom racunaru ne radi. Da bi radilo mora da bude url: "/ListaStanica/ListaUnutrasnjihStanica"

                $.ajax({
                    type: "GET",
                    url: baseUrl + "/ListaStanica/ListaUnutrasnjihStanica",
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

            $('#DatumOd, #DatumDo, #OtpDatum, #Datum, #k165aDatum').datepicker({
                todayBtn: true,
                todayBtn: 'linked',
                todayHighlight: true,
                format: 'dd.mm.yyyy',
                autoclose: true,
                language: 'rs'
            })


            //// Example starter JavaScript for disabling form submissions if there are invalid fields
            //(function () {
            //    'use strict'

            // Fetch all the forms we want to apply custom Bootstrap validation styles to
            var forms = document.querySelectorAll('.needs-validation')                                                      // za validaciju polja na formi

            // Loop over them and prevent submission
            Array.prototype.slice.call(forms)
                .forEach(function (form) {
                    form.addEventListener('submit', function (event) {
                        if (!form.checkValidity()) {
                            event.preventDefault()
                            event.stopPropagation()
                        }

                        form.classList.add('was-validated')
                    }, false)
                })
            /*  })()*/









           // var arr = [];

            //function Lista() {

            //    $.ajax({
            //        type: "GET",
            //        url: "/ListaStanica/ListaMedjunarodnihStanica",
            //        cache: false,
            //        success: function (data) {

            //            for (var i = 0; i < data.length; i++) {
            //                arr.push(data[i])
            //            }
                        
                        
            //        }
            //    });
            //}

            //Lista()

            //$('#myDatalistInputMedju').on('input', function () {

            //    var data = $('#myDatalistInputMedju').val()

            //    console.log(data)

            //    var filtered = arr.filter(function (value) {
            //        return value.indexOf(data) > -1
            //    })

            //    var options = '';

            //    for (var i = 0; i < filtered.length; i++) {
            //        options += '<option value="' + filtered[i] + '" />';
            //    }
            //    $('#myDatalist').html(options)

            //    console.log(filtered)
                

            //})

            


        })


