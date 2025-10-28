import {ComponentProps, ReactNode, useState} from "react";
import Button from "@/components/button.tsx";
import {Check, Copy} from "lucide-react";

// A button that copies text to clipboard on click
export default function ClipboardButton({ getText, icon, clickIcon, ...props } : {
    icon?: ReactNode,
    clickIcon?: ReactNode,
    getText: () => string
} & Omit<ComponentProps<typeof Button>, 'onClick'>) {

    const [clicked, setClicked] = useState(false);
    const onClick = async () => {
        await navigator.clipboard.writeText(getText());

        setClicked(true);
        setTimeout(() => setClicked(false), 1000);
    }

    return <Button onClick={onClick} {...props}>
        {clicked ? (clickIcon ?? <Check />) : (icon ?? <Copy />)}
    </Button>
}