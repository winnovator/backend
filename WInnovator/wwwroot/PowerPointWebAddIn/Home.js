
(function () {
    "use strict";

    var messageBanner;
    var token;
    var dropdownDesignShopInitialising;
    var dropdownDesignShopWorkingFormInitialising;
    var callbackArgument;
    var imagesInSelectedWorkingForm;
    var currentImage;
    var numberofImages;

    // The initialize function must be run each time a new page is loaded.
    Office.initialize = function (reason) {
        $(document).ready(function () {
            // Initialize the notification mechanism and hide it
            var element = document.querySelector('.MessageBanner');
            messageBanner = new components.MessageBanner(element);
            messageBanner.hideBanner();

            // Set the designShops
            getDesignShops();

            $('#insert-image').click(insertImage);
            $('#designshop-dropdown').change(function () {
                if (dropdownDesignShopInitialising == false) {
                    $('#werkvorm').removeClass('hide');
                    getDesignShopWorkingForms($(this).val());
                    $('#designshop-dropdown').blur();
                }
            });
            $('#designshopworkingform-dropdown').change(function () {
                if (dropdownDesignShopWorkingFormInitialising == false) {
                    getImagesForSelectedWorkingForm($(this).val());
                    $('#designshopworkingform-dropdown').blur();
                }
            });
        });
    };

    function getDesignShops() {
        getTokenWithCallback(getDesignShopsWithToken);
    }

    function getDesignShopsWithToken() {
        dropdownDesignShopInitialising = true;
        let dropdown = $('#designshop-dropdown');

        dropdown.empty();

        dropdown.append('<option selected="true" disabled>Selecteer een designshop</option>');
        dropdown.prop('selectedIndex', 0);

        $.ajax({
            headers: {
                'Authorization': 'Bearer ' + token
            },
            url: "/api/DesignShop", success: function (data) {
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').attr('value', entry.id).text(entry.description));
                })
            }, error: function (xhr, status, error) {
                showNotification("Error", "Error retrieving designshops.");
            }
        });
        dropdownDesignShopInitialising = false;
    }

    function getDesignShopWorkingForms(designshopId) {
        getTokenWithCallback(getDesignShopWorkingFormsWithToken, designshopId);
    }

    function getDesignShopWorkingFormsWithToken(designshopId) {
        dropdownDesignShopWorkingFormInitialising = true;
        let dropdown = $('#designshopworkingform-dropdown');

        dropdown.empty();

        dropdown.append('<option selected="true" disabled>Selecteer een werkvorm</option>');
        dropdown.prop('selectedIndex', 0);

        $.ajax({
            headers: {
                'Authorization': 'Bearer ' + token
            },
            url: "/api/WorkingForm/" + designshopId, success: function (data) {
                $.each(data, function (key, entry) {
                    dropdown.append($('<option></option>').attr('value', entry.id).text(entry.description));
                })
            }, error: function (xhr, status, error) {
                showNotification("Error", "Error retrieving designshopworkingforms.");
            }
        });
        dropdownDesignShopWorkingFormInitialising = false;
    }

    function getImagesForSelectedWorkingForm(workingformId) {
        getTokenWithCallback(getImagesForSelectedWorkingFormWithToken, workingformId);
    }

    function getImagesForSelectedWorkingFormWithToken(workingformId) {
        $.ajax({
            headers: {
                'Authorization': 'Bearer ' + token
            },
            url: "/api/WorkingForm/" + workingformId + "/imageList", success: function (data) {
                imagesInSelectedWorkingForm = data;
                let aantal = imagesInSelectedWorkingForm.length;
                $('#imagesDescription').html("Voeg " + aantal + " vrije " + ((aantal == 1) ? "pagina" : "pagina's") + " toe en selecteer " + ((aantal == 1) ? "deze." : "de eerste die leeg is.") + " Klik daarna op onderstaande button.")
                $('#imagesButtontext').html("Voeg <b>" + aantal + "</b> " + ((aantal == 1) ? "afbeelding" : "afbeeldingen") + " toe")
                toggleImagesAreas(aantal != 0, aantal == 0);
            }, error: function (xhr, status, error) {
                showNotification("Error", "Error retrieving designshopworkingforms.");
            }
        });
    }

    function toggleImagesAreas(showImagearea, showNoimagearea) {
        if ($('#imagesarea').hasClass('hide') && showImagearea) {
            $('#imagesarea').removeClass('hide');
        }
        if (!($('#imagesarea').hasClass('hide')) && !showImagearea) {
            $('#imagesarea').addClass('hide');
        }
        if ($('#noimagesarea').hasClass('hide') && showNoimagearea) {
            $('#noimagesarea').removeClass('hide');
        }
        if (!($('#noimagesarea').hasClass('hide')) && !showNoimagearea) {
            $('#noimagesarea').addClass('hide');
        }
    }

    function insertImage() {
        getTokenWithCallback(insertAllImagesWithToken);
    }

    function insertAllImagesWithToken() {
        numberofImages = imagesInSelectedWorkingForm.length;
        currentImage = -1;
        processImage();
    }

    function processImage() {
        currentImage++;
        if (currentImage < numberofImages) {
            insertImageWithToken(imagesInSelectedWorkingForm[currentImage].id);
        }
    }

    function insertImageWithToken(imageId) {
        // Get image from from web service (as a Base64 encoded string).
        $.ajax({
            headers: {
                'Authorization': 'Bearer ' + token
            },
            url: "/api/DownloadImage/" + imageId, success: function (result) {
                insertImageFromBase64String(result);
            }, error: function (xhr, status, error) {
                showNotification("Error", "Oops, something went wrong.");
            }
        });
    }

    function insertImageFromBase64String(image) {
        // Call Office.js to insert the image into the document.
        Office.context.document.setSelectedDataAsync(image, {
            coercionType: Office.CoercionType.Image
        },
            function (asyncResult) {
                if (asyncResult.status === Office.AsyncResultStatus.Failed) {
                    showNotification("Error", asyncResult.error.message);
                } else {
                    nextSlide();
                }
            });
    }

    function nextSlide() {
        //var goToFirst = Office.Index.First;
        //var goToLast = Office.Index.Last;
        //var goToPrevious = Office.Index.Previous;
        var goToNext = Office.Index.Next;

        if (currentImage < numberofImages - 1) {
            Office.context.document.goToByIdAsync(goToNext, Office.GoToType.Index, function (asyncResult) {
                if (asyncResult.status == "failed") {
                    showNotification("Error", "Unable to go to the next slide due to error: " + asyncResult.error.message);
                } else {
                    processImage();
                }
            });
        }
    }

    function getTokenWithCallback(_callback) {
        token = "";
        callbackArgument = "";
        if (arguments.length > 1) {
            callbackArgument = arguments[1];
        }
        var credData = {
            email: 'email',
            password: 'password'
        }

        $.ajax({
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(credData),
            dataType: 'json',
            url: "/api/Token", success: function (result) {
                token = result.token;
                if (callbackArgument == "") {
                    _callback();
                } else {
                    _callback(callbackArgument);
                }
            }, error: function (xhr, status, error) {
                showNotification("Error retrieving token", "Error retrieving valid token, statuscode: " + xhr.status);
            }
        });
    }

    // PREVIOUS
    // Reads data from current document selection and displays a notification
    function getDataFromSelection() {
        Office.context.document.getSelectedDataAsync(Office.CoercionType.Text,
            function (result) {
                if (result.status === Office.AsyncResultStatus.Succeeded) {
                    showNotification('The selected text is:', '"' + result.value + '"');
                } else {
                    showNotification('Error:', result.error.message);
                }
            }
        );
    }

    // Helper function for displaying notifications
    function showNotification(header, content) {
        $("#notification-header").text(header);
        $("#notification-body").text(content);
        messageBanner.showBanner();
        messageBanner.toggleExpansion();
    }
})();
