export function getBarcode(dotNetHelper) {

    // Square QR box with edge size = 80% of the smaller edge of the viewfinder.
    let qrboxFunction = function (viewfinderWidth, viewfinderHeight) {
        let minEdgePercentage = 0.8; // 80%
        let minEdgeSize = Math.min(viewfinderWidth, viewfinderHeight);
        let qrboxSize = Math.floor(minEdgeSize * minEdgePercentage);
        return {
            width: qrboxSize,
            height: qrboxSize
        };
    }

    var html5QrcodeScanner = new Html5QrcodeScanner(
        "qr-reader", {
        fps: 10,
        qrbox: qrboxFunction,
        aspectRatio: 1.0,
        showTorchButtonIfSupported: true,
        showZoomSliderIfSupported: true,
        defaultZoomValueIfSupported: 2
    })

    function onScanSuccess(decodedText) {

        dotNetHelper.invokeMethodAsync('OnSuccessAsync', decodedText);

        // Optional: To close the QR code scannign after the result is found
        //html5QrcodeScanner.clear();

    }

    // Optional callback for error, can be ignored.
    function onScanError(qrCodeError) {
        // This callback would be called in case of qr code scan error or setup error.
        // You can avoid this callback completely, as it can be very verbose in nature.
    }

    html5QrcodeScanner.render(onScanSuccess, onScanError);
}