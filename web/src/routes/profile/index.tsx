import { createFileRoute } from '@tanstack/react-router'
import FileUpload from "@/components/file-upload.tsx";
import {patchKoworkersMe, postUploads} from "koworking-shared/api";
import {client, uploadUrl} from "@/utils/backend.ts";
import {useQuery, useQueryClient} from "@tanstack/react-query";
import currentUserOptions from "@/routes/-auth/current-user-options.ts";
import Card from "@/components/card.tsx";
import UserAvatar from "@/components/user-avatar.tsx";
import Loading from "@/components/loading.tsx";
import Button from "@/components/button.tsx";
import {Trash} from "lucide-react";

export const Route = createFileRoute('/profile/')({
  component: RouteComponent,
})

function RouteComponent() {

  const { data: user } = useQuery({
    ...currentUserOptions(),
    select: data => data?.user
  })

  const queryClient = useQueryClient();
  const setAvatar = async (image: File | null) => {
    const imageUrl = image
        ? await postUploads({ client, throwOnError: true,
          body: {
            File: image,
            Scope: "Avatar"
          }}).then(x => uploadUrl(x.data))
        : null;
    await patchKoworkersMe({ client, throwOnError: true,
      body: {
        avatarUrl: imageUrl
      }
    });
    await queryClient.refetchQueries({ queryKey: currentUserOptions.queryKey });
  }

  if (!user) {
    return <Loading />
  }

  return <div className='max-w-lg flex flex-col gap-3 justify-center mx-auto'>
    <Card grow={false} className='gap-2'>
      <h2 className='text-2xl text-center font-semibold'>Аватар</h2>
      <UserAvatar
          user={user}
          className='rounded-2xl overflow-hidden w-full h-120 max-h-svw text-4xl'
          imageProps={{ className: 'h-120 max-h-svw' }}/>
      <div className='flex flex-row gap-1'>
        <div className='w-4/5'>
          <FileUpload accept='image/*' onUpload={files => setAvatar(files.at(0)!)} />
        </div>
        <div className='w-1/5'>
          <Button className='bg-red-800 hover:bg-red-900 shrink' onClick={() => setAvatar(null)}>
            <Trash />
          </Button>
        </div>
      </div>
    </Card>
  </div>
}
