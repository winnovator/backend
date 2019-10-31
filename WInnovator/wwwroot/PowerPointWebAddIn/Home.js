(function () {
    "use strict";

    var messageBanner;

    Office.onReady(function () {
        $(document).ready(function () {
            // Initialize the FabricUI notification mechanism and hide it
            //var element = document.querySelector('.MessageBanner');
            //messageBanner = new components.MessageBanner(element);
            //messageBanner.hideBanner();

            $('#insert-image').click(insertImage);
            // TODO4: Assign event handler for insert-text button.
            // TODO6: Assign event handler for get-slide-metadata button.
            // TODO8: Assign event handlers for the four navigation buttons.
        });
    });

    function insertImage() {
        // Get image from from web service (as a Base64 encoded string).
        $.ajax({
            url: "/api/DownloadImage/e0a23ab7-dc74-42bc-bf92-08d75e3c68a2", success: function (result) {
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
                }
            });
    }

    // TODO5: Define the insertText function.

    // TODO7: Define the getSlideMetadata function.

    // TODO9: Define the navigation functions.

    // Helper function for displaying notifications
    function showNotification(header, content) {
        $("#notification-header").text(header);
        $("#notification-body").text(content);
        messageBanner.showBanner();
        messageBanner.toggleExpansion();
    }
})();