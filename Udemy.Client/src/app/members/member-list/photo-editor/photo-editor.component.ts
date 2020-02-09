import { environment } from "./../../../../environments/environment";
import { Component, OnInit, Input } from "@angular/core";
import { Photo } from "src/app/_models/photo";
import { FileUploader } from "ng2-file-upload";
import { AuthService } from "src/app/_services/auth.service";
import { JsonPipe } from "@angular/common";

@Component({
  selector: "app-photo-editor",
  templateUrl: "./photo-editor.component.html",
  styleUrls: ["./photo-editor.component.css"]
})
export class PhotoEditorComponent implements OnInit {
  @Input() photos: Photo[];
  uploader: FileUploader;
  hasBaseDropZoneOver: true;
  response: string;
  baseUrl = environment.apiUrl;

  constructor(private authService: AuthService) {}

  ngOnInit() {
    this.initializeUploader();
  }

  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url:
        this.baseUrl +
        "users/" +
        this.authService.decodedToken.nameid +
        "/photos",
      authToken: "Bearer " + localStorage.getItem("token"),
      isHTML5: true, // 'DisableMultipart' must be 'true' for formatDataFunction to be called.
      allowedFileType: ["image"],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onSuccessItem = (file, response, status, headers) => {
      if (response) {
        const res: Photo = JSON.parse(response);
        const photo: Photo = {
          id: res.id,
          url: res.url,
          dateAdded: res.dateAdded,
          descriptioon: res.descriptioon,
          isMain: res.isMain
        };
        this.photos.push(photo);
      }
    };
    this.uploader.onAfterAddingFile = file => {
      file.withCredentials = false;
    };
  }
}
