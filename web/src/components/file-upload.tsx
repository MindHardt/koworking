import {ComponentProps, useCallback, useRef, useState, ClipboardEvent as ReactClipboardEvent} from "react";
import Button from "@/components/button";
import {Dialog} from "@base-ui-components/react";


export default function FileUpload({ onUpload, accept, multiple, ...props } : {
    onUpload: (files: File[]) => void | Promise<void>,
    multiple?: ComponentProps<'input'>['multiple'],
    accept?: ComponentProps<'input'>['accept']
} & ComponentProps<typeof Button>) {

    const dropZoneRef = useRef<HTMLDivElement>(null);
    const inputRef = useRef<HTMLInputElement>(null);
    const [isDragging, setDragging] = useState(false);
    const [open, setOpen] = useState(false);
    const [files, setFiles] = useState<File[]>([]);

    const onOpenChange = useCallback((open: boolean) => {
        open && dropZoneRef.current?.focus();
        setOpen(open);
    }, [dropZoneRef.current]);
    const onPaste = useCallback((e : ReactClipboardEvent<HTMLDivElement>) => {
        if (inputRef.current) {
            inputRef.current.files = e.clipboardData?.files ?? inputRef.current.files;
            inputRef.current.dispatchEvent(new Event('change'));
        }
    }, [inputRef]);
    const startUpload = () => {
        onUpload(files);
        setOpen(false);
    }

    return <>
        <input type='file' multiple={multiple} accept={accept} ref={inputRef} hidden onChange={e => setFiles([...e.target.files!])}/>
        <Dialog.Root open={open} onOpenChange={onOpenChange}>
            <Dialog.Trigger render={<div></div>} nativeButton={false}>
                <Button {...props}>Загрузите файл{multiple && 'ы'}</Button>
            </Dialog.Trigger>
            <Dialog.Portal>
                <Dialog.Backdrop className='fixed inset-0 min-w-dvw min-h-dvh bg-black opacity-20 dark:opacity-70 transition-[opacity] data-ending-style:opacity-0 data-starting-style:opacity-0 duration-150 ease-in' />
                <Dialog.Popup className='fixed top-1/2 left-1/2 -translate-1/2 w-96 max-w-dvw bg-white border-gray-300 shadow p-2 rounded-2xl'>
                    <Dialog.Title className='text-2xl text-center pb-2'>Загрузите файлы</Dialog.Title>
                    <div
                        ref={dropZoneRef}
                        className={'p-2 border border-dashed rounded-lg h-32 flex justify-center items-center ring-main-50 ring-4 ' + (isDragging ? 'border-main-500' : 'border-gray-300')}
                        onPaste={onPaste}
                        onDragEnter={() => setDragging(true)}
                        onDragLeave={() => setDragging(false)}
                        onDrop={e => setFiles([...e.dataTransfer.files])}>
                        <Button className='h-8 w-1/2 text-sm' onClick={() => inputRef.current?.click()}>Выберите файл</Button>
                    </div>
                    <div className='p-2'>
                        <Button className='h-12' disabled={files.length === 0} onClick={startUpload}>
                            Загрузить
                        </Button>
                    </div>
                </Dialog.Popup>
            </Dialog.Portal>
        </Dialog.Root>
    </>
}