import * as winston from 'winston';
import * as seq from '@datalust/winston-seq';
if (typeof window !== "undefined") {
    throw new Error('Attempted to import logger client-side')
}

const logger = new winston.Logger({
    level: 'info',
    format: winston.format.combine(
        winston.format.errors({ stack: true }),
        winston.format.json(),
    ),
    defaultMeta: { 'Application': 'kowoking-web' },
    transports: [
        new winston.transports.Console({
            format: winston.format.cli({ all: true })
        }),
        new seq.SeqTransport({
            serverUrl: process.env.SEQ_URL,
            onError: (e => { console.error(e) }),
            handleExceptions: true,
            handleRejections: true
        })
    ]
});

export default logger;