const FileUploader = (function () {
    const ALLOWED_EXTENSIONS = ["csv", "txt"];
    const MAX_FILE_SIZE = 5 * 1024 * 1024; // 5MB
    const UPLOAD_ENDPOINT = "/Home/UploadFile";

    function init() {

        const uploadButton = document.getElementById("uploadBtn");
        if (uploadButton) {
            uploadButton.addEventListener("click", uploadFile);
        }
    }

    async function uploadFile() {
        hideError();

        // Get file
        const fileInput = document.getElementById("fileInput");
        const file = fileInput.files[0];

        if (!file) {
            showError("Please select a file.");
            return;
        }

        if (!validateFile(file)) {
            return;
        }

        const formData = new FormData();
        formData.append("File", file);

        const tokenElement = document.querySelector("input[name='__RequestVerificationToken']");

        try {
            const response = await fetch(UPLOAD_ENDPOINT, {
                method: "POST",
                body: formData,
                headers: {
                    "X-CSRF-TOKEN": tokenElement.value
                }
            });

            let result = await response.json();
            if (response.ok && result.success) {
                window.location.href = result.redirectUrl;
            }
            else {
                showError(result.message || "An error occurred.");
                resetFileInput();
            }
        } catch (error) {
            console.error("Request failed:", error);
            resetFileInput();
        }
    }

    function validateFile(file) {

        const extension = file.name.split(".").pop().toLowerCase();
        if (!ALLOWED_EXTENSIONS.includes(extension)) {
            showError("Invalid file type. Only CSV and TXT are allowed.");
            return false;
        }

        if (file.size > MAX_FILE_SIZE) {
            showError("File size exceeds 5MB.");
            return false;
        }

        return true;
    }

    function showError(message) {
        let errorMessageDiv = document.getElementById("errorMessage");

        if (!errorMessageDiv) {
            errorMessageDiv = document.createElement("div");
            errorMessageDiv.id = "errorMessage";
            errorMessageDiv.className = "alert alert-danger";

            const formContainer = document.getElementById("upload-form-container");
            formContainer.insertBefore(errorMessageDiv, formContainer.firstChild);
        }

        errorMessageDiv.textContent = message;
        errorMessageDiv.style.display = "block";
    }

    function hideError() {
        let errorMessageDiv = document.getElementById("errorMessage");

        if (errorMessageDiv) {
            errorMessageDiv.style.display = "none";
        }
    }

    function resetFileInput() {
        let fileInput = document.getElementById("fileInput");
        fileInput.value = "";
    }

    return {
        init,
        uploadFile
    };
})();

document.addEventListener("DOMContentLoaded", FileUploader.init);