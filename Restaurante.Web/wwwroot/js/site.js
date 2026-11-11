// Bootstrap toasts do not show themselves: this finds the ones the server rendered and shows them.
document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.app-toast').forEach(function (element) {
        bootstrap.Toast.getOrCreateInstance(element).show();
    });
});