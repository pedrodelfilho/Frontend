$(document).ready(function () {

    function readURL(input) {
        if (input.files && input.files[0]) {

            var reader = new FileReader();

            reader.onload = function (e) {
                $('.image-upload-wrap').hide();

                $('.file-upload-image').attr('src', e.target.result);
                $('.file-upload-content').show();

                $('.image-title').html(input.files[0].name);
            };

            reader.readAsDataURL(input.files[0]);

        } else {
            removeUpload();
        }
    }
    function removeUpload() {
        $('.file-upload-input').replaceWith($('.file-upload-input').clone());
        $('.file-upload-content').hide();
        $('.image-upload-wrap').show();
    }
    function habilitarSelectConvenio() {
        if ($('#radioConvenio').is(':checked')) {
            $("#selectConvenio").prop("disabled", false);
        }
        else {
            $("#selectConvenio").prop("disabled", true);
        }
    }
    function inicializar() {
        $('.image-upload-wrap').bind('dragover', function () {
            $('.image-upload-wrap').addClass('image-dropping');
        });
        $('.image-upload-wrap').bind('dragleave', function () {
            $('.image-upload-wrap').removeClass('image-dropping');
        });
        $('input[type=radio][name=radioParticularConvenio]').change(function (e) {
            e.preventDefault();
            habilitarSelectConvenio();
        });
        $('.file-upload-input').change(function (e) {
            e.preventDefault();
            readURL(this);
        });
        habilitarSelectConvenio();
    }
    inicializar();
});
