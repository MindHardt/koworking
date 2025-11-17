import {
    ComponentProps,
    useCallback,
    useRef,
    useState,
    ClipboardEvent as ReactClipboardEvent,
    useEffect,
    ChangeEvent
} from "react";
import Button from "@/components/button";
import {Dialog} from "@base-ui-components/react";
import {File as FileIcon, Upload, X} from "lucide-react";
import {cn} from "@/utils/cn.ts";

export default function FileUpload({ onUpload, accept, multiple, ...props } : {
    onUpload: (files: ReadonlyArray<File>) => void | Promise<void>,
    multiple?: ComponentProps<'input'>['multiple'],
    accept?: ComponentProps<'input'>['accept']
} & ComponentProps<typeof Button>) {

    const dropZoneRef = useRef<HTMLDivElement>(null);
    const inputRef = useRef<HTMLInputElement>(null);
    const [isDragging, setDragging] = useState(false);
    const [open, setOpen] = useState(false);
    const [files, setFiles] = useState<File[]>([]);

    const contentTypeMatches = useCallback(({ type } : { type: string }) => {
        if (accept === undefined || accept === '*/*') {
            return true;
        }
        if (accept.includes('/*')) {
            return accept.substring(0, accept.indexOf('/')) == type.substring(0, type.indexOf('/'))
        }
        return accept == type;
    }, [accept]);
    const onFilesSelected = useCallback((e: ChangeEvent<HTMLInputElement>) => {
        const newFiles = [...(e.target.files ?? [])];
        setFiles(prev => multiple ? [...prev, ...newFiles] : [...newFiles])
    }, [multiple])
    const onOpenChange = useCallback((open: boolean) => {
        open && dropZoneRef.current?.focus();
        setOpen(open);
    }, [dropZoneRef.current]);
    const onPaste = useCallback((e : ReactClipboardEvent<HTMLDivElement>) => {
        if (inputRef.current) {
            const matchingFiles = [...e.clipboardData?.files].filter(contentTypeMatches)

            if (matchingFiles.length !== 0) {
                setFiles(prev => multiple ? [...prev, ...matchingFiles] : [...matchingFiles])
            }
        }
    }, [inputRef, contentTypeMatches]);
    const startUpload = () => {
        onUpload(files);
        setFiles([]);
        setOpen(false);
    }

    return <>
        <input type='file' multiple={multiple} accept={accept} ref={inputRef} hidden onChange={onFilesSelected}/>
        <Dialog.Root open={open} onOpenChange={onOpenChange}>
            <Dialog.Trigger render={<div></div>} nativeButton={false}>
                <Button {...props}>Загрузите файл{multiple && 'ы'}</Button>
            </Dialog.Trigger>
            <Dialog.Portal>
                <Dialog.Backdrop className='fixed inset-0 min-w-dvw min-h-dvh bg-black opacity-20 dark:opacity-70 transition-[opacity] data-ending-style:opacity-0 data-starting-style:opacity-0 duration-150 ease-in' />
                <Dialog.Popup className='fixed top-1/2 left-1/2 -translate-1/2 w-144 max-w-dvw bg-white border-gray-300 shadow p-2 rounded-2xl'>
                    <Dialog.Title className='text-2xl text-center pb-2'>Загрузите файлы</Dialog.Title>
                    <div
                        ref={dropZoneRef}
                        className={'p-2 border border-dashed rounded-lg h-48 flex justify-center items-center ring-main-50 ring-4 ' + (isDragging ? 'border-main-500' : 'border-gray-300')}
                        onPaste={onPaste}
                        onDragEnter={() => setDragging(true)}
                        onDragLeave={() => setDragging(false)}
                        onDrop={e => setFiles([...e.dataTransfer.files])}>
                        {files.length !== 0 &&
                            <div className='p-3 grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3'>
                                {files.map((file, i) => <Preview
                                    className={multiple ? '' : 'col-span-full'} key={file.name} file={file}
                                    remove={(() => {
                                        setFiles(prev => [...prev].splice(i + 1, 1));
                                    })}
                                />)}
                            </div>}
                        {(multiple || files.length === 0) &&
                            <div className='flex flex-col gap-1 items-center w-full'>
                                <Button className='h-10 w-1/2 text-sm' onClick={() => inputRef.current?.click()}>
                                    Выберите файл
                                </Button>
                                <span className='text-gray-500 text-sm'>Или вставьте из буфера обмена</span>
                            </div>}
                    </div>
                    <div className='p-2'>
                        <Button className='h-12 flex flex-row gap-1' disabled={files.length === 0} onClick={startUpload}>
                            <Upload />
                            Отправить
                        </Button>
                    </div>
                </Dialog.Popup>
            </Dialog.Portal>
        </Dialog.Root>
    </>
}

function Preview({ className, file, remove, ...props } : {
    file: File,
    remove: () => void
} & ComponentProps<'div'>) {

    const [url, setUrl] = useState<string>();
    useEffect(() => {
        if (file.type.startsWith('image/')) {
            setUrl(URL.createObjectURL(file));
        }
        return () => {
            url && URL.revokeObjectURL(url);
            setUrl(undefined);
        }
    }, [file])

    return <div className={cn('overflow-hidden w-full h-36 rounded-2xl border-main-300 relative', className)} {...props}>
        <div role='button' className='absolute right-0 rounded-2xl m-1 bg-gray-100 hover:bg-gray-200 size-8'
             onClick={remove}>
            <X className='p-1 size-full text-red-500 hover:text-red-600' />
        </div>
        {url
            ? <img src={url} alt={file.name} className='size-full'/>
            : <div className='flex flex-col gap-1 items-center justify-center'>
                <FileIcon />
                <span className='font-mono text-lg'>{file.name}</span>
            </div>}
    </div>;

}