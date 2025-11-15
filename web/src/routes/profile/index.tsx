import { createFileRoute } from '@tanstack/react-router'
import FileUpload from "@/components/file-upload.tsx";
import {patchKoworkersMe, postUploads} from "koworking-shared/api";
import {client, uploadUrl} from "@/utils/backend.ts";
import {useQueryClient} from "@tanstack/react-query";
import currentUserOptions from "@/routes/-auth/current-user-options.ts";

export const Route = createFileRoute('/profile/')({
  component: RouteComponent,
})

function RouteComponent() {

  const queryClient = useQueryClient();
  const onUploadAvatar = async (files: File[]) => {
    const image = files.pop()!;
    const imageUrl = await postUploads({
      client,
      throwOnError: true,
      body: {
        File: image,
        Scope: "Avatar"
      }}).then(x => uploadUrl(x.data));
    await patchKoworkersMe({
      client,
      throwOnError: true,
      body: {
        avatarUrl: imageUrl
      }
    });
    await queryClient.refetchQueries({ queryKey: currentUserOptions.queryKey });
  }

  return <FileUpload accept='image/*' onUpload={onUploadAvatar} />
}
