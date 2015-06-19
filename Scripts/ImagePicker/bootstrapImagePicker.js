//! bootstrapImagePicker.js
//! version : 0.0.1
//! authors : Didiet Eka Permana
//! license : MIT

if (typeof jQuery === 'undefined') {
    throw new Error('ImagePicker\'s JavaScript requires jQuery')
}

+function ($) {
    var version = $.fn.jquery.split(' ')[0].split('.')
    if ((version[0] < 2 && version[1] < 9) || (version[0] == 1 && version[1] == 9 && version[2] < 1)) {
        throw new Error('ImagePicker\'s JavaScript requires jQuery version 1.9.1 or higher')
    }
}(jQuery);

+function ($) {
    'use strict';

    var RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();

    var ImagePicker = function (element, options) {
        this.options = options;

        this.pickerName = element.id;

        this.init(element);
    }

    ImagePicker.VERSION = '0.0.1'

    ImagePicker.prototype = {
        constructor: ImagePicker,

        init: function (element) {
            $(element).append(this.templateHTML());

            this.getData();
        },

        templateHTML: function() {
            return ' \
            <div class="' + this.pickerName + 'Picker modal fade"> \
                <div class="modal-dialog"> \
                    <div class="modal-content"> \
                        <div class="modal-header"> \
                            <button type="button" class="close" data-dismiss="modal" data-toggle="modal" data-target="#' + this.options.caller + 'AddEditModal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button> \
                            <h4 class="modal-title">' + this.options.title + '</h4> \
                        </div> \
                        <div class="modal-body row"> \
                        </div> \
                        <div class="modal-footer"> \
                            <a class="btn btn-default" data-dismiss="modal" data-toggle="modal" data-target="#' + this.options.caller + 'AddEditModal">Close</a> \
                        </div> \
                    </div> \
                </div> \
            </div> \
            ';
        },

        getData: function () {
            var that = this;
            $.post(this.options.pickerGetData, { "__RequestVerificationToken": RequestVerificationToken }, function (result) {
                if (result.total > 0) {
                    var data = result.payload;
                    var i = 0;

                    $('.' + that.pickerName + 'Picker .modal-body').empty();

                    for (; i < data.length; i++) {
                        $('.' + that.pickerName + 'Picker .modal-body').append('<div class="' + that.pickerName + 'imageLoc col-md-2 col-xs-3" data-url="' + data[i] + '"><a class="thumbnail"><img src="' + data[i] + '" width="50" height="50"></a></div>');
                    }

                    $('.' + that.pickerName + 'imageLoc').click(that, that.imagePick);
                }
                else if (result.errors != null && result.errors.length > 0) {
                    alert(result.errors);
                }
                else {
                    alert("Generic error");
                }
            }).fail(function (jqXHR, textStatus, errorThrown) {
                alert("Got some error: " + errorThrown);
            });
        },

        imagePick: function (arg) {
            var that = arg.data;
            var url = $(this).data("url");

            $('.' + that.pickerName + 'Picker').modal('hide');
            $('#input' + that.options.callerInput).attr('src', url);
            $('#' + that.options.caller + 'AddEditModal').modal();
        }
    }

    // IMAGEPICKER PLUGIN DEFINITION
    // ==========================
    function Plugin(option) {
        return this.each(function () {
            var data = new ImagePicker(this, option);
        })
    }

    var old = $.fn.imagePicker;
    $.fn.imagePicker = Plugin;

    // IMAGEPICKER NO CONFLICT
    // ====================
    $.fn.imagePicker.noConflict = function () {
        $.fn.grid = old
        return this
    }
}(jQuery)