import binascii
import hmac
import struct
import argparse

parser = argparse.ArgumentParser(description='Process A FCKING GRP PACKET FCKING HATE THIS BULLSHIT.')
parser.add_argument('hex', metavar='N', type=str, nargs='+', help='The hex')
args = parser.parse_args()
hexval = args.hex[0]
data = bytearray.fromhex(hexval)
data = data.ljust((len(data) + 3) & ~3, b"\0")
words = struct.unpack("<%iI" %(len(data) // 4), data)
result = ((sum(b'cH0on9AsIXx7') & 0xFF) + int(sum(words))) & 0xffffffff
hexresult = hex(result).lstrip("0x").rjust(8,"0")
swappedresult = hexresult[-2] + hexresult[-1] + hexresult[-4] + hexresult[-3] + hexresult[-6] + hexresult[-5] + hexresult[-8] + hexresult[-7]
print(swappedresult)